namespace Rpd.Interv.Core.Data.Models.View;

/// <summary> 
/// A view model representing the total time worked by an employee.
/// </summary>
public class EmployeeChartDataViewModel
{
    /// <summary>
    /// The name of the employee.
    /// </summary>
    public List<string> EmployeeNames { get; set; } = [];

    /// <summary>
    /// The total number of hours worked by the employee.
    /// </summary>
    public List<double> HoursWorked { get; set; } = [];
}
