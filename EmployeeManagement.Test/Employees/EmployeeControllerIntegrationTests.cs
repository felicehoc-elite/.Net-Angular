using EmployeeManagement.Api.Controllers;
using EmployeeManagement.Core.Models;
using EmployeeManagement.Core.Repositories;
using EmployeeManagement.Core.Services;
using EmployeeManagement.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Core.Helper;
using EmployeeManagement.Core.DTO;

namespace EmployeeManagement.Test.Employees
{
    [TestFixture]
    public class EmployeeControllerIntegrationTests
    {
        private DbContextOptions<EmployeeManagementContext> _options;
        private EmployeeManagementContext _context;

        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<EmployeeManagementContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new EmployeeManagementContext(_options);
        }

        [Test]
        public async Task Get_ShouldReturnOkResultForExistingEmployeeAsync()
        {
            // Arrange
            string existingEmployeeId = InitializeEmployee();

            var employeeService = new EmployeeService(new EmployeeRepository(_context));
            var employeeController = new EmployeeController(employeeService);

            // Act
            var result = await employeeController.GetEmployeeForEdit(existingEmployeeId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);

            ClearEmployees();
        }

        [Test]
        public async Task Get_ShouldReturnNotFoundResultForNonExistingEmployeeAsync()
        {
            // Arrange
            var nonExistingEmployeeId = IdGenerator.NewId();
            var employeeService = new EmployeeService(new EmployeeRepository(_context));
            var employeeController = new EmployeeController(employeeService);

            // Act
            var result = await employeeController.GetEmployeeForEdit(nonExistingEmployeeId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task GetList_ShouldReturnOkResultForEmployeesAsync()
        {
            // Arrange
            InitializeEmployee();

            var employeeService = new EmployeeService(new EmployeeRepository(_context));
            var employeeController = new EmployeeController(employeeService);

            // Act
            var result = await employeeController.GetList(null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);

            ClearEmployees();
        }

        [Test]
        public async Task Post_ShouldReturnCreatedAtActionResultAsync()
        {
            // Arrange
            var newEmployee = new EmployeeCreate { FirstName = "Jane", LastName = "Smith", RoleIds = new int[] { 1, 2 } };
            var employeeService = new EmployeeService(new EmployeeRepository(_context));
            var employeeController = new EmployeeController(employeeService);

            // Act
            var result = await employeeController.Post(newEmployee);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task Put_ShouldReturnOkResultForUpdatedEmployeeAsync()
        {
            // Arrange
            var existingEmployeeId = IdGenerator.NewId();
            var employee = new Employee(existingEmployeeId) { FirstName = "John", LastName = "Doe" };
            _context.Employees.Add(employee);
            _context.SaveChanges();

            var updatedEmployee = new EmployeeEdit { Id = existingEmployeeId, FirstName = "UpdatedJohn", LastName = "Doe" };
            var employeeService = new EmployeeService(new EmployeeRepository(_context));
            var employeeController = new EmployeeController(employeeService);

            // Act
            var result = await employeeController.Put(updatedEmployee);
            var getResult = await employeeController.GetEmployeeForEdit(existingEmployeeId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(getResult);
            Assert.IsInstanceOf<OkResult>(result);
            Assert.IsInstanceOf<OkObjectResult>(getResult.Result);

            ClearEmployees();
        }

        [Test]
        public async Task Delete_ShouldReturnOkResultForDeletedEmployeeAsync()
        {
            // Arrange
            var existingEmployeeId = IdGenerator.NewId();
            var employee = new Employee(existingEmployeeId) { FirstName = "John", LastName = "Doe" };
            _context.Employees.Add(employee);
            _context.SaveChanges();

            var employeeService = new EmployeeService(new EmployeeRepository(_context));
            var employeeController = new EmployeeController(employeeService);

            // Act
            var result = await employeeController.Delete(existingEmployeeId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkResult>(result);

            ClearEmployees();
        }

        private string InitializeEmployee()
        {
            var existingEmployeeId = IdGenerator.NewId(); ;
            var employee = new Employee(existingEmployeeId) { FirstName = "John", LastName = "Doe" };
            _context.Employees.Add(employee);
            _context.SaveChanges();
            return existingEmployeeId;
        }

        private void ClearEmployees() 
        {
            _context.Employees.RemoveRange(_context.Employees);
            _context.SaveChanges();
        }
    }
}
