using EmployeeManagement.Core.Models;

namespace EmployeeManagement.Core.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly EmployeeManagementContext _context;

    public RoleRepository(EmployeeManagementContext employeeManagementContext)
    {
        _context = employeeManagementContext;
    }

    public async Task AddAsync(Role role)
    {
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();
    }

    public IQueryable<Role> GetAll()
    {
        return _context.Roles.AsQueryable();
    }
}