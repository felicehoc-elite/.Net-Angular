using EmployeeManagement.Core.DTO;
using EmployeeManagement.Core.Models;
using EmployeeManagement.Core.Repositories;
using EmployeeManagement.Core.Services;
using Moq;

namespace EmployeeManagement.Test.Employees
{
    [TestFixture]
    public class EmployeeServiceTests
    {
        private Mock<IEmployeeRepository> _mockEmployeeRepository;
        private IEmployeeService _employeeService;

        [SetUp]
        public void Setup()
        {
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _employeeService = new EmployeeService(_mockEmployeeRepository.Object);
        }

        [Test]
        public async Task AddEmployeeWithRolesAsync_ShouldAddEmployeeToRepositoryAsync()
        {
            // Arrange
            var employeeCreate = new EmployeeCreate { FirstName = "John", LastName = "Doe", RoleIds = new int[] { 1, 2 } };

            // Act
            await _employeeService.AddEmployeeWithRolesAsync(employeeCreate);

            // Assert
            _mockEmployeeRepository.Verify(repo =>
                repo.AddEmployeeWithRolesAsync(
                    It.IsAny<Employee>(),
                    It.IsAny<List<int>>()), Times.Once);
        }

        [Test]
        public async Task UpdateEmployeeWithRolesAsync_NullEmployee_ShouldThrowArgumentNullException()
        {
            // Arrange
            string id = "1";
            EmployeeEdit nullEmployee = null;

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _employeeService.UpdateEmployeeWithRolesAsync(nullEmployee));
        }

        //[Test]
        //public async Task DeleteAsync_NonexistentEmployee_ShouldThrowNotFoundException()
        //{
        //    // Arrange
        //    string id = IdGenerator.NewId();
        //    _mockEmployeeRepository.Setup(repo => repo.GetEmployees(null))
        //        .Returns(new List<Employee>().AsQueryable());

        //    // Act & Assert
        //    await Assert.Throws<NotFoundException>(async() => 
        //        await _employeeService.DeleteAsync(id));
        //}
    }
}
