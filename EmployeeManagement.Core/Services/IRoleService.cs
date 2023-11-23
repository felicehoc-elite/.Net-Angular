using EmployeeManagement.Core.DTO;

namespace EmployeeManagement.Core.Services
{
    public interface IRoleService
    {
        Task<List<RoleView>> GetRolesAsync();
        Task<List<string>> GetRoleNamesAsync(int[] roleIds);
    }
}
