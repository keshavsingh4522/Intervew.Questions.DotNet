using Microsoft.AspNetCore.Mvc;
using Rpd.Interv.Core.Abstraction;
using Rpd.Interv.Core.Data.Models.View;
using Rpd.Interv.WebApp.Controllers;

namespace Rpd.Interv.Test.Controllers;

[TestFixture]
public class EmployeeControllerTests
{
    private Mock<ILogger<EmployeeController>> _loggerMock;
    private Mock<ITimeEntryService> _timeEntryServiceMock;
    private EmployeeController _controller;

    [SetUp]
    public void Setup()
    {
        // Mock the logger
        _loggerMock = new Mock<ILogger<EmployeeController>>();

        // Mock the service
        _timeEntryServiceMock = new Mock<ITimeEntryService>();

        // Initialize the controller with the mocked dependencies
        _controller = new EmployeeController(_loggerMock.Object, _timeEntryServiceMock.Object);
    }

    [Test]
    public async Task Test_Index_ReturnsViewResult_WithListOfEmployeeTimeViewModels()
    {
        // Arrange
        var fakeData = new List<EmployeeTimeViewModel>
        {
            new() { EmployeeName = "John Doe", TotalHoursWorked = 120 },
            new() { EmployeeName = "Jane Smith", TotalHoursWorked = 95 }
        };
        _timeEntryServiceMock.Setup(service => service.CalculateTotalHours())
            .ReturnsAsync(fakeData);

        // Act
        var result = await _controller.Index();

        // Assert
        Assert.That(result, Is.InstanceOf<ViewResult>());
        var viewResult = result as ViewResult;
        Assert.That(actual: viewResult?.Model,
                    Is.InstanceOf<List<EmployeeTimeViewModel>>());
        List<EmployeeTimeViewModel>? model = viewResult.Model as List<EmployeeTimeViewModel>;
        Assert.That(model, Has.Count.EqualTo(2));
    }
}