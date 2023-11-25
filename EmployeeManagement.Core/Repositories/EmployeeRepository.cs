using EmployeeManagement.Core.Exceptions;
using EmployeeManagement.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Core.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly EmployeeManagementContext _context;

    public EmployeeRepository(EmployeeManagementContext employeeManagementContext)
    {
        _context = employeeManagementContext;
    }

    public async Task AddAsync(Employee employee)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
    }
        
    public IQueryable<Employee> GetEmployees(string? managerId)
    {
        var query = _context.Employees.AsQueryable();
        if (managerId != null)
        {
            query = query.Where(_ => _.ManagerId == managerId);
        }

        return query;
    }
    
    public async Task AddEmployeeWithRolesAsync(Employee employee, List<int> roleIds)
    {
        employee.Roles = new List<EmployeeRole>();

        var rolesToAdd = await _context.Roles.Where(r => roleIds.Contains(r.Id)).ToListAsync();

        var employeeRoles = rolesToAdd.Select(role => new EmployeeRole
        {
            Employee = employee,
            Role = role
        }).ToList();

        foreach (var employeeRole in employeeRoles)
        {
            employee.Roles.Add(employeeRole);
        }

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateEmployeeWithRolesAsync(Employee employee, List<int> roleIds)
    {
        var existingEmployee = await _context.Employees.Include(_ => _.Roles)
            .FirstOrDefaultAsync(e => e.Id == employee.Id);

        if (existingEmployee is null)
        {
            throw new NotFoundException($"Employee {employee.Id} not found");
        }

        existingEmployee.Roles.Clear();

        var rolesToAdd = await _context.Roles.Where(r => roleIds.Contains(r.Id)).ToListAsync();

        var employeeRoles = rolesToAdd.Select(role => new EmployeeRole
        {
            Employee = existingEmployee,
            Role = role
        }).ToList();

        foreach (var employeeRole in employeeRoles)
        {
            existingEmployee.Roles.Add(employeeRole);
        }

        existingEmployee.FirstName = employee.FirstName;
        existingEmployee.LastName = employee.LastName;
        existingEmployee.ManagerId = employee.ManagerId;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var employeeToDelete = _context.Employees
                                        .Include(e => e.Roles)
                                        .FirstOrDefault(e => e.Id == id);

        if (employeeToDelete != null)
        {
            foreach (var role in employeeToDelete.Roles.ToList())
            {
                employeeToDelete.Roles.Remove(role);
            }

            _context.Employees.Remove(employeeToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
