using EmployeeManagement.Core.Models;

namespace EmployeeManagement.Core.Repositories;

public interface IRoleRepository
{
    IQueryable<Role> GetAll();
    Task AddAsync(Role role);
}
