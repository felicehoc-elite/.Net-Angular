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
            _mockEmployeeService.Setup(service => service.GetAsync(existingEmployeeId))
                .ReturnsAsync(new EmployeeCreate { Id = existingEmployeeId });

            // Act
            var result = await _employeeController.Get(existingEmployeeId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task Get_ShouldReturnOkResultWithEmployeeAsync()
        {
            // Arrange
            var existingEmployeeId = "1";
            var employeeData = new EmployeeCreate { Id = existingEmployeeId, /* other employee data */ };
            _mockEmployeeService.Setup(service => service.GetAsync(existingEmployeeId))
                .ReturnsAsync(employeeData);

            // Act
            var result = await _employeeController.Get(existingEmployeeId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            Assert.IsInstanceOf<EmployeeCreate>(okResult.Value);
            // Add more assertions as needed
        }

        [Test]
        public async Task GetList_ShouldReturnNotFoundResultWhenNoEmployeesExist()
        {
            // Arrange
            _mockEmployeeService.Setup(service => service.GetAllAsync(null))
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
            var employeeResult = expectedResult ? new EmployeeCreate() { Id = employeeId } : null;
            _mockEmployeeService.Setup(service => service.GetAsync(employeeId))
                .ReturnsAsync(employeeResult);

            // Act
            var result = await _employeeController.Get(employeeId);

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
