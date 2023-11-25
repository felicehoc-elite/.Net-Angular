using EmployeeManagement.Core.DTO;

namespace EmployeeManagement.Core.Services
{
    public interface IEmployeeService
    {
        Task<EmployeeCreate> GetAsync(string id);
        Task<List<EmployeeView>> GetAllAsync(string? managerId);
        Task<List<EmployeeView>> GetAllManagersAsync();
        Task AddEmployeeWithRolesAsync(EmployeeCreate employee);
        Task UpdateEmployeeWithRolesAsync(EmployeeCreate employee);
        Task DeleteAsync(string id);
    }
}
