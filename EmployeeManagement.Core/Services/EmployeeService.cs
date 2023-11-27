using EmployeeManagement.Core.Constants;
using EmployeeManagement.Core.DTO;
using EmployeeManagement.Core.Exceptions;
using EmployeeManagement.Core.Models;
using EmployeeManagement.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EmployeeManagement.Core.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        
        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<EmployeeEdit> GetEmployeeForEditAsync(string id)
        {
            var employee = await _employeeRepository.GetEmployees(null).Include(_ => _.Roles)
                .Where(_ => _.Id == id)
                .Select(_ => new EmployeeEdit()
                {
                    Id = id,
                    FirstName = _.FirstName,
                    LastName = _.LastName,
                    ManagerId = _.ManagerId,
                    RoleIds = _.Roles.Select(r => r.RoleId).ToArray()
                })
                .FirstOrDefaultAsync();

            if (employee is null)
            {
                throw new NotFoundException($"Employee {id} not found");
            }

            return employee;
        }

        public async Task<List<EmployeeView>> GetEmployeesAsync(string? managerId)
        {
            return await _employeeRepository.GetEmployees(managerId)
                .Include(_ => _.Roles)
                .Select(MapToEmployeeView())
                .ToListAsync();
        }

        public async Task AddEmployeeWithRolesAsync(EmployeeCreate employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            var employeeEntity = employee.Entity();
            await _employeeRepository.AddEmployeeWithRolesAsync(employeeEntity, employee.RoleIds.ToList());
        }

        public async Task<List<EmployeeView>> GetAllManagersAsync()
        {
            return await _employeeRepository.GetEmployees(null)
                .Include(_ => _.Roles)
                .Where(_ => _.Roles.Any(r => r.Role.Name == RoleType.Director))
                .Select(MapToEmployeeView())
                .ToListAsync();
        }

        public async Task UpdateEmployeeWithRolesAsync(EmployeeEdit employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee), "Employee cannot be null");
            }

            await _employeeRepository
                .UpdateEmployeeWithRolesAsync(employee.Entity(), employee.RoleIds.ToList());
        }

        public async Task DeleteAsync(string id)
        {
            var employeeToDelete = await _employeeRepository.GetEmployees(null)
                .FirstOrDefaultAsync(_ => _.Id == id);

            if (employeeToDelete == null)
            {
                throw new NotFoundException($"Employee {id} not found");
            }

            await _employeeRepository.DeleteAsync(id);
        }

        private static Expression<Func<Employee, EmployeeView>> MapToEmployeeView()
        {
            return _ => new EmployeeView()
            {
                Id = _.Id,
                FirstName = _.FirstName,
                LastName = _.LastName,
                Roles = _.Roles.Select(r => r.Role.Name).ToArray()
            };
        }
    }
}
