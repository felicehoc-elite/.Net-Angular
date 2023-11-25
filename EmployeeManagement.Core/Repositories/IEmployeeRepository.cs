using EmployeeManagement.Core.Models;

namespace EmployeeManagement.Core.Repositories;
public interface IEmployeeRepository
{
    IQueryable<Employee> GetEmployees(string? managerId);
    Task AddAsync(Employee employee);
    Task AddEmployeeWithRolesAsync(Employee employee, List<int> roleIds);
    Task UpdateEmployeeWithRolesAsync(Employee employee, List<int> roleIds);
    Task DeleteAsync(string id);
}
