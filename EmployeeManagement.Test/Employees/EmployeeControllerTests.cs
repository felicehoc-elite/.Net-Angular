using EmployeeManagement.Api.Controllers;
using EmployeeManagement.Core.DTO;
using EmployeeManagement.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeManagement.Test.Employees
{
    [TestFixture]
    public class EmployeeControllerTests
    {
        private Mock<IEmployeeService> _mockEmployeeService;
        private EmployeeController _employeeController;

        [SetUp]
        public void Setup()
        {
            _mockEmployeeService = new Mock<IEmployeeService>();
            _employeeController = new EmployeeController(_mockEmployeeService.Object);
        }

        [Test]
        public async Task Get_ExistingEmployeeId_ShouldReturnOkResult()
        {
            // Arrange
            var existingEmployeeId = "1";
            _mockEmployeeService.Setup(service => service.GetEmployeeForEditAsync(existingEmployeeId))
                .ReturnsAsync(new EmployeeEdit { Id = existingEmployeeId });

            // Act
            var result = await _employeeController.GetEmployeeForEdit(existingEmployeeId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task Get_ShouldReturnOkResultWithEmployeeAsync()
        {
            // Arrange
            var existingEmployeeId = "1";
            var employeeData = new EmployeeEdit { Id = existingEmployeeId, /* other employee data */ };
            _mockEmployeeService.Setup(service => service.GetEmployeeForEditAsync(existingEmployeeId))
                .ReturnsAsync(employeeData);

            // Act
            var result = await _employeeController.GetEmployeeForEdit(existingEmployeeId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            Assert.IsInstanceOf<EmployeeEdit>(okResult.Value);
        }

        [Test]
        public async Task GetList_ShouldReturnNotFoundResultWhenNoEmployeesExist()
        {
            // Arrange
            _mockEmployeeService.Setup(service => service.GetEmployeesAsync(null))
                .ReturnsAsync(new List<EmployeeView>());

            // Act
            var result = await _employeeController.GetList(null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task Post_ValidEmployee_ShouldReturnOkResult()
        {
            // Arrange
            var validEmployee = new EmployeeCreate
            { FirstName = "John", LastName = "Doe", RoleIds = new int[] { 1, 2 } };
            _mockEmployeeService.Setup(service => service.AddEmployeeWithRolesAsync(validEmployee))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _employeeController.Post(validEmployee);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [TestCase("1", true)]
        [TestCase("2", false)]
        public async Task Get_ExistingEmployeeId_ShouldReturnExpectedResult(string employeeId, bool expectedResult)
        {
            // Arrange
            var employeeResult = expectedResult ? new EmployeeEdit() { Id = employeeId } : null;
            _mockEmployeeService.Setup(service => service.GetEmployeeForEditAsync(employeeId))
                .ReturnsAsync(employeeResult);

            // Act
            var result = await _employeeController.GetEmployeeForEdit(employeeId);

            // Assert
            if (expectedResult)
            {
                Assert.IsNotNull(result);
                Assert.IsInstanceOf<OkObjectResult>(result.Result);
            }
            else
            {
                Assert.IsNotNull(result);
                Assert.IsInstanceOf<NotFoundResult>(result.Result);
            }
        }
    }
}
