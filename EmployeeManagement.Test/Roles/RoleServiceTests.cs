using EmployeeManagement.Core.Repositories;
using EmployeeManagement.Core.Services;
using Moq;

namespace EmployeeManagement.Test.Roles
{
    [TestFixture]
    public class RoleServiceTests
    {
        private Mock<IRoleRepository> _mockRoleRepository;
        private IRoleService _roleService;

        [SetUp]
        public void Setup()
        {
            _mockRoleRepository = new Mock<IRoleRepository>();
            _roleService = new RoleService(_mockRoleRepository.Object);
        }

        [Test]
        public async Task GetRoleNamesAsync_WithNoRoleIds_ShouldReturnEmptyList()
        {
            // Arrange
            var roleIds = new int[] { };

            // Act
            var result = await _roleService.GetRoleNamesAsync(roleIds);

            // Assert
            Assert.IsNotNull(result);
            CollectionAssert.IsEmpty(result);
        }
    }
}
