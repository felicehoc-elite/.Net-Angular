using EmployeeManagement.Api.Controllers;
using EmployeeManagement.Core.Constants;
using EmployeeManagement.Core.DTO;
using EmployeeManagement.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeManagement.Test.Roles
{
    [TestFixture]
    public class RoleControllerTests
    {
        private Mock<IRoleService> _mockRoleService;
        private RoleController _roleController;

        [SetUp]
        public void Setup()
        {
            _mockRoleService = new Mock<IRoleService>();
            _roleController = new RoleController(_mockRoleService.Object);
        }

        [Test]
        public async Task GetRoles_ExistingRoles_ShouldReturnOkResult()
        {
            // Arrange
            _mockRoleService.Setup(service => service.GetRolesAsync())
                .ReturnsAsync(new List<RoleView> { new RoleView { Id = 1, Name = RoleType.Support} });

            // Act
            var result = await _roleController.GetRoles();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }
    }
}
