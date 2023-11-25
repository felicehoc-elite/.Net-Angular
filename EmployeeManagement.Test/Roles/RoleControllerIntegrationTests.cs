using EmployeeManagement.Api.Controllers;
using EmployeeManagement.Core;
using EmployeeManagement.Core.Constants;
using EmployeeManagement.Core.DTO;
using EmployeeManagement.Core.Models;
using EmployeeManagement.Core.Repositories;
using EmployeeManagement.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace EmployeeManagement.Test.Roles
{
    [TestFixture]
    public class RoleControllerIntegrationTests
    {
        private RoleController _roleController;
        private IRoleService _roleService;

        [SetUp]
        public void Setup()
        {
            var serviceProvider = new ServiceCollection()
                .AddDbContext<EmployeeManagementContext>(options => options.UseInMemoryDatabase("TestDatabase"))
                .AddScoped<IRoleRepository, RoleRepository>()
                .AddScoped<IRoleService, RoleService>()
                .BuildServiceProvider();

            _roleService = serviceProvider.GetRequiredService<IRoleService>();
            _roleController = new RoleController(_roleService);
        }

        [Test]
        public async Task GetRoles_ShouldReturnOkResultWithRoleViews()
        {
            // Arrange
            List<Role> roles = GetMockRoles();

            using (var dbContext = new EmployeeManagementContext(
                new DbContextOptionsBuilder<EmployeeManagementContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options))
            {
                dbContext.Roles.AddRange(roles);
                dbContext.SaveChanges();
            }

            // Act
            var result = await _roleController.GetRoles();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);

            var okResult = (OkObjectResult)result.Result;
            var roleViews = ((IEnumerable<RoleView>)okResult.Value).OrderBy(_ => _.Id).ToList();

            var expectedRoleViews = roles.OrderBy(_ => _.Id).Select(r => new RoleView { Id = r.Id, Name = r.Name }).ToList();

            Assert.AreEqual(expectedRoleViews.Count, roleViews.Count(), "Collection counts are not equal.");

            for (int i = 0; i < expectedRoleViews.Count; i++)
            {
                Assert.IsTrue(AreRoleViewsEqual(expectedRoleViews[i], roleViews.ElementAt(i)), $"Element at index {i} is not equal.");
            }

            CleanMockRoleTable();
        }

        [Test]
        public async Task GetRoles_ShouldReturnNotFoundResultWhenNoRolesExist()
        {
            // Act
            var result = await _roleController.GetRoles();

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task GetRoleNames_ShouldReturnOkResultWithRoleNames()
        {
            // Arrange
            var roleIds = new[] { 1, 2, 3 };
            var expectedRoleNames = new[] { RoleType.Director, RoleType.IT, RoleType.Support };

            using (var dbContext = new EmployeeManagementContext(
                new DbContextOptionsBuilder<EmployeeManagementContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options))
            {
                // Adding roles to the in-memory database
                var roles = roleIds.Select((id, index) => new 
                    Role { Id = id, Name = expectedRoleNames[index] }).ToList();
                dbContext.Roles.AddRange(roles);
                dbContext.SaveChanges();
            }

            // Act
            var result = await _roleController.GetRoleNames(roleIds);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);

            var okResult = (OkObjectResult)result.Result;
            var roleNames = (IEnumerable<string>)okResult.Value;
            CollectionAssert.AreEquivalent(expectedRoleNames, roleNames);

            CleanMockRoleTable();
        }

        [Test]
        public async Task GetRoleNames_ShouldReturnNotFoundResultWhenNoRolesExist()
        {
            // Arrange
            var roleIds = new int[] { };

            // Act
            var result = await _roleController.GetRoleNames(roleIds);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task GetRoleNames_ShouldReturnBadRequestResultOnException()
        {
            // Arrange
            var roleIds = new[] { 1, 2, 3 };

            // Simulate an exception in the service
            var roleService = new Mock<IRoleService>();
            roleService.Setup(s => s.GetRoleNamesAsync(roleIds))
                .Throws(new Exception("Simulated exception"));

            _roleController = new RoleController(roleService.Object);

            // Act
            var result = await _roleController.GetRoleNames(roleIds);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }

        private static List<Role> GetMockRoles()
        {
            return new List<Role>
            {
                new Role { Id = 1, Name = RoleType.Director },
                new Role { Id = 2, Name = RoleType.IT },
                new Role { Id = 3, Name = RoleType.Support }
            };
        }

        private static void CleanMockRoleTable()
        {
            using (var dbContext = new EmployeeManagementContext(
                            new DbContextOptionsBuilder<EmployeeManagementContext>()
                            .UseInMemoryDatabase("TestDatabase")
                            .Options))
            {
                dbContext.Roles.RemoveRange(dbContext.Roles);
                dbContext.SaveChanges();
            }
        }

        bool AreRoleViewsEqual(RoleView expected, RoleView actual)
        {
            return expected.Id == actual.Id && expected.Name == actual.Name;
        }
    }
}
