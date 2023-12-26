using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Rpd.Interv.Core.Abstraction;

namespace Rpd.Interv.WebApp.Controllers;

/// <summary>
/// Controller for the Employee views.
/// </summary>
public class EmployeeController(ILogger<EmployeeController> logger, ITimeEntryService timeEntryService) : Controller
{
    // Inject the logger and service dependencies.
    private readonly ILogger<EmployeeController> _logger = logger;

    // Inject the service dependency.
    private readonly ITimeEntryService _timeEntryService = timeEntryService;

    /// <summary>
    /// Asynchronously retrieves time entries from the service and passes them to the view.
    /// </summary>
    public async Task<IActionResult> Index()
    {
        // Log the action call.
        _logger.LogInformation("Index action called.");

        // Call the service to retrieve the time entries.
        var employeeTimeViewModels = await _timeEntryService.CalculateTotalHours();

        // Log the data.
        _logger.LogInformation("EmployeeTimeViewModels: {@employeeTimeViewModels}", employeeTimeViewModels);

        // Pass the data to the view.
        return View(employeeTimeViewModels);
    }

    /// <summary>
    /// Asynchronously retrieves time entries from the service and passes them to the view.
    /// </summary>
    public async Task<IActionResult> PieChart()
    {
        // Log the action call.
        _logger.LogInformation("PieChart action called.");

        // Call the service to retrieve the time entries.
        var employeeTimeViewModels = await _timeEntryService.CalculateTotalHours();

        // Pass the data to the view.
        ViewBag.DataPoints = JsonConvert.SerializeObject(employeeTimeViewModels);

        // Log the data.
        _logger.LogInformation("EmployeeTimeViewModels: {@employeeTimeViewModels}", employeeTimeViewModels);

        return View();
    }
}
