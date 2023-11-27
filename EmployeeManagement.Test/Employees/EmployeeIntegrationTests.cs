using EmployeeManagement.Core;
using EmployeeManagement.Core.DTO;
using EmployeeManagement.Core.Helper;
using EmployeeManagement.Core.Models;
using EmployeeManagement.Core.Repositories;
using EmployeeManagement.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagement.Test.Employees
{
    [TestFixture]
    public class EmployeeIntegrationTests
    {
        private IEmployeeService _employeeService;
        private EmployeeManagementContext _dbContext;

        [SetUp]
        public void Setup()
        {
            var serviceProvider = new ServiceCollection()
                .AddDbContext<EmployeeManagementContext>(_ => _.UseInMemoryDatabase("TestDatabase"), ServiceLifetime.Scoped)
                .AddScoped<IEmployeeRepository, EmployeeRepository>()
                .AddScoped<IEmployeeService, EmployeeService>()
                .BuildServiceProvider();

            _employeeService = serviceProvider.GetRequiredService<IEmployeeService>();
            _dbContext = serviceProvider.GetRequiredService<EmployeeManagementContext>();
        }

        [Test]
        public async Task GetEmployeeForEditAsync_ExistingEmployeeId_ShouldReturnEmployeeCreate()
        {
            // Arrange
            var existingEmployeeId = IdGenerator.NewId();
            var employees = new List<Employee>
            {
                new Employee(existingEmployeeId) { FirstName = "John", LastName = "Doe" }
            };

            _dbContext.Employees.AddRange(employees);
            _dbContext.SaveChanges();

            // Act
            var result = await _employeeService.GetEmployeeForEditAsync(existingEmployeeId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<EmployeeEdit>(result);

            ClearEmployees();
        }

        [Test]
        public async Task GetEmployeesAsync_ShouldReturnListOfEmployeeViews()
        {
            Console.Write($"{IdGenerator.NewId()} = {IdGenerator.NewId()} = {IdGenerator.NewId()}");

            // Arrange
            var employees = new List<Employee>
            {
                new Employee(IdGenerator.NewId()) { FirstName = "John", LastName = "Doe" },
                new Employee(IdGenerator.NewId()) { FirstName = "Jane", LastName = "Smith" },
                new Employee(IdGenerator.NewId()) { FirstName = "Dan", LastName = "Smith" }
            };

            _dbContext.Employees.AddRange(employees);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _employeeService.GetEmployeesAsync(null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<EmployeeView>>(result);
            Assert.IsTrue(result.Count > 0);

            ClearEmployees();
        }

        [Test]
        public async Task AddEmployeeAsync_ShouldCreateNewEmployee()
        {
            // Arrange
            var newEmployee = new EmployeeCreate { FirstName = "New", LastName = "Employee" };

            // Act
            await _employeeService.AddEmployeeWithRolesAsync(newEmployee);

            // Assert
            var createdEmployee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.FirstName == "New" && e.LastName == "Employee");
            Assert.IsNotNull(createdEmployee);
        }

        [Test]
        public async Task UpdateEmployeeAsync_ShouldUpdateExistingEmployee()
        {
            // Arrange
            var existingEmployeeId = IdGenerator.NewId();
            var existingEmployee = new Employee(existingEmployeeId) { FirstName = "John", LastName = "Doe" };
            _dbContext.Employees.Add(existingEmployee);
            _dbContext.SaveChanges();

            var updatedEmployee = new EmployeeEdit
                { Id = existingEmployeeId, FirstName = "UpdatedJohn", LastName = "Doe" };

            // Act
            await _employeeService.UpdateEmployeeWithRolesAsync(updatedEmployee);

            // Assert
            var updatedEmployeeEntity = await _dbContext.Employees.FindAsync(existingEmployeeId);
            Assert.IsNotNull(updatedEmployeeEntity);
            Assert.AreEqual("UpdatedJohn", updatedEmployeeEntity.FirstName);
        }

        [Test]
        public async Task DeleteEmployeeAsync_ShouldDeleteExistingEmployee()
        {
            // Arrange
            var existingEmployeeId = IdGenerator.NewId();
            var existingEmployee = new Employee(existingEmployeeId) { FirstName = "John", LastName = "Doe" };
            _dbContext.Employees.Add(existingEmployee);
            _dbContext.SaveChanges();

            // Act
            await _employeeService.DeleteAsync(existingEmployeeId);

            // Assert
            var deletedEmployee = await _dbContext.Employees.FindAsync(existingEmployeeId);
            Assert.IsNull(deletedEmployee);
        }

        private void ClearEmployees()
        {
            _dbContext.Employees.RemoveRange(_dbContext.Employees);
            _dbContext.SaveChanges();
        }
    }
}
