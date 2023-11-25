using EmployeeManagement.Core.DTO;
using EmployeeManagement.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        
        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<List<string>> GetRoleNamesAsync(int[] roleIds)
        {
            if (roleIds == null || !roleIds.Any())
            {
                return new List<string>();
            }

            return await _roleRepository.GetAll()
                .Where(role => roleIds.Contains(role.Id))
                .Select(role => role.Name)
                .ToListAsync();
        }

        public async Task<List<RoleView>> GetRolesAsync()
        {
            return await _roleRepository.GetAll()
                .Select(role => new RoleView { Id = role.Id, Name = role.Name })
                .ToListAsync();
        }
    }
}
