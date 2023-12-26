using Rpd.Interv.Core.Data.Models;
using Rpd.Interv.Core.Data.Models.View;

namespace Rpd.Interv.Core.Abstraction;

/// <summary>
/// Interface for the TimeEntryService.
/// </summary>
public interface ITimeEntryService
{
    /// <summary>
    /// Asynchronously retrieves time entries from an external API.
    /// </summary>
    /// <returns>A list of TimeEntry objects.</returns>
    Task<List<TimeEntry>> GetTimeEntriesAsync();

    Task<List<EmployeeTimeViewModel>> CalculateTotalHours();
}