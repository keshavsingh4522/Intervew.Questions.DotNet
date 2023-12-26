using System.Runtime.Serialization;

namespace Rpd.Interv.Core.Data.Models.View;

/// <summary>
/// A view model representing the total time worked by an employee.
/// </summary>
[DataContract]
public class EmployeeTimeViewModel
{
    /// <summary>
    /// The name of the employee.
    /// </summary>
    [DataMember(Name = "label")]
    public string? EmployeeName { get; set; }

    /// <summary>
    /// The total number of hours worked by the employee.
    /// </summary>
    [DataMember(Name = "y")]
    public double TotalHoursWorked { get; set; }

    /// <summary>
    /// Indicates whether the employee's time entry should be highlighted due to working fewer than 100 hours.
    /// </summary>
    public bool ShouldHighlight { get; set; }
}