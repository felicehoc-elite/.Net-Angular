using EmployeeManagement.Core;
using EmployeeManagement.Core.Constants;
using EmployeeManagement.Core.Models;
using EmployeeManagement.Core.Repositories;
using EmployeeManagement.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagement.Test.Roles
{
    [TestFixture]
    public class RoleServiceIntegrationTests
    {
        private IRoleService _roleService;

        [SetUp]
        public void Setup()
        {
            var serviceProvider = new ServiceCollection()
                .AddDbContext<EmployeeManagementContext>(options =>
                    options.UseInMemoryDatabase(databaseName: "InMemoryDb"))
                .AddScoped<IRoleRepository, RoleRepository>()
                .AddScoped<IRoleService, RoleService>()
                .BuildServiceProvider();

            _roleService = serviceProvider.GetRequiredService<IRoleService>();
        }



        [Test]
        public async Task GetRoleNamesAsync_ShouldReturnListOfRoleNamesAsync()
        {
            // Arrange
            var roleIds = new[] { 1, 2, 3 };
            var roleData = new List<Role>
            {
                new Role { Id = 1, Name = RoleType.Director },
                new Role { Id = 2, Name = RoleType.IT },
                new Role { Id = 3, Name = RoleType.Support }
            };

            using (var dbContext = new EmployeeManagementContext(
                new DbContextOptionsBuilder<EmployeeManagementContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .Options))
            {
                dbContext.Roles.AddRange(roleData);
                dbContext.SaveChanges();
            }

            // Act
            var result = await _roleService.GetRoleNamesAsync(roleIds);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<string>>(result);

            Assert.AreEqual(3, result.Count);
            Assert.Contains("Director", result);
            Assert.Contains("IT", result);
            Assert.Contains("Support", result);

            using (var dbContext = new EmployeeManagementContext(
                new DbContextOptionsBuilder<EmployeeManagementContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .Options))
            {
                // Ensure the database is clean
                dbContext.Roles.RemoveRange(dbContext.Roles);
                dbContext.SaveChanges();
            }
        }

        [Test]
        public async Task GetRolesAsync_ShouldReturnEmptyListWhenNoRolesExist()
        {
            // Act
            var result = await _roleService.GetRolesAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }
    }
}
