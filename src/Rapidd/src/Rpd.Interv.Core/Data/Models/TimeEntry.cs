using System.Text.Json.Serialization;

namespace Rpd.Interv.Core.Data.Models;

/// <summary>
/// Represents an individual time entry record for an employee.
/// </summary>
public class TimeEntry
{
    /// <summary>
    /// Unique identifier for the time entry.
    /// </summary>
    [JsonPropertyName("Id")]
    public string Id { get; set; }

    /// <summary>
    /// Name of the employee associated with the time entry.
    /// </summary>
    [JsonPropertyName("EmployeeName")]
    public string? EmployeeName { get; set; }

    /// <summary>
    /// The start time of the work period in Coordinated Universal Time (UTC).
    /// </summary>
    [JsonPropertyName("StarTimeUtc")]
    public string? StartTimeUtc { get; set; }

    /// <summary>
    /// The end time of the work period in Coordinated Universal Time (UTC).
    /// </summary>
    [JsonPropertyName("EndTimeUtc")]
    public string? EndTimeUtc { get; set; }

    /// <summary>
    /// Notes detailing the work performed during the time entry.
    /// </summary>
    [JsonPropertyName("EntryNotes")]
    public string? EntryNotes { get; set; }

    /// <summary>
    /// The timestamp when the time entry was marked as deleted, if applicable.
    /// </summary>
    [JsonPropertyName("DeletedOn")]
    public string? DeletedOn { get; set; }
}