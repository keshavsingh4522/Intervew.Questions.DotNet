using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Rpd.Interv.Core.Abstraction;
using Rpd.Interv.Core.Data.Models;
using Rpd.Interv.Core.Data.Models.View;
using System.Text.Json;
namespace Rpd.Interv.Core.Services;

/// <summary>
/// Service to interact with the Time Entries API.
/// </summary>
public class TimeEntryService(ILogger<TimeEntryService> logger, IHttpClientFactory clientFactory, IConfiguration configuration) : ITimeEntryService
{
    private readonly ILogger<TimeEntryService> _logger = logger;
    private readonly IHttpClientFactory _clientFactory = clientFactory;
    private readonly IConfiguration _configuration = configuration;

    /// <summary>
    /// Asynchronously retrieves time entries from an external API.
    /// </summary>
    /// <returns>A list of TimeEntry objects.</returns>
    public async Task<List<TimeEntry>> GetTimeEntriesAsync()
    {
        try
        {
            // Create an HttpClient instance using the factory with the named configuration "TimeEntryHttpClient".
            HttpClient client = _clientFactory.CreateClient("TimeEntryHttpClient");

            // Define the request URI.
            string requestUri = ""; //

            // Send the GET request to the specified URI.
            HttpResponseMessage response = await client.GetAsync(requestUri);

            // Ensure the response is successful.
            response.EnsureSuccessStatusCode();

            // Read the response content as a string.
            string jsonResponse = await response.Content.ReadAsStringAsync();

            // Deserialize the JSON response into a list of TimeEntry objects.
            List<TimeEntry>? timeEntries = JsonSerializer.Deserialize<List<TimeEntry>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Return the list of time entries or an empty list if null.
            return timeEntries ?? [];
        }
        catch (HttpRequestException e)
        {
            // Log the exception if the request fails.
            _logger.LogError("Error fetching time entries: {Message}", e.Message);
            return []; // Return an empty list rather than null to avoid potential null reference issues.
        }
        catch (JsonException e)
        {
            // Log the exception if JSON deserialization fails.
            _logger.LogError("Error deserializing time entries: {Message}", e.Message);
            return []; // Return an empty list on deserialization failure.
        }
    }

    /// <summary>
    /// Calculates the total hours worked for each employee from a list of time entries.
    /// </summary>
    /// <param name="timeEntries">The list of time entries.</param>
    /// <returns>A list of EmployeeTimeViewModel objects.</returns>
    public async Task<List<EmployeeTimeViewModel>> CalculateTotalHours()
    {
        List<TimeEntry> timeEntries = await GetTimeEntriesAsync();
        // Group time entries by employee name and calculate the total hours worked
        List<EmployeeTimeViewModel> value = [
            .. timeEntries.GroupBy(e => e.EmployeeName)
                                              //.Select(group => new EmployeeTimeViewModel
                                              //{
                                              //    EmployeeName = group.Key,
                                              //    TotalHoursWorked = group.Sum(g =>
                                              //        (DateTime.Parse(g.EndTimeUtc) - DateTime.Parse(g.StartTimeUtc)).TotalHours),
                                              //    ShouldHighlight = group.Sum(g =>
                                              //        (DateTime.Parse(g.EndTimeUtc) - DateTime.Parse(g.StartTimeUtc)).TotalHours) < 100
                                              //})
                                              .Select(group =>
                                              {
                                                  // Initialize total hours worked to zero
                                                  double totalHoursWorked = 0;

                                                  foreach (var entry in group)
                                                  {
                                                      // Attempt to parse start and end times. If either is invalid, skip this entry.
                                                      if (DateTime.TryParse(entry.StartTimeUtc, out var start) && DateTime.TryParse(entry.EndTimeUtc, out var end))
                                                      {
                                                          // Ensure that the end time is not before the start time.
                                                          if (end > start)
                                                          {
                                                              // Accumulate the total hours worked.
                                                              totalHoursWorked += (end - start).TotalHours;
                                                          }
                                                          else
                                                          {
                                                              // Optionally log a warning or handle the case where end is before start.
                                                              _logger.LogWarning("The end time is before the start time for entry ID {EntryId}.", entry.Id);
                                                          }
                                                      }
                                                      else
                                                      {
                                                          // Optionally log a warning or handle the case where the dates are not parsable.
                                                          _logger.LogWarning("Unable to parse start or end time for entry ID {EntryId}.", entry.Id);
                                                      }
                                                  }

                                                  // Return the view model for this group (employee).
                                                  return new EmployeeTimeViewModel
                                                  {
                                                      EmployeeName = group.Key ?? "Other",
                                                      TotalHoursWorked = Math.Round(totalHoursWorked, 2),
                                                      ShouldHighlight = totalHoursWorked < 100
                                                  };
                                              })
                                              .OrderByDescending(e => e.TotalHoursWorked)
        ,
        ];
        return value;
    }
}
