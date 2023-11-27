using EmployeeManagement.Core.DTO;

namespace EmployeeManagement.Core.Services
{
    public interface IEmployeeService
    {
        Task<EmployeeEdit> GetEmployeeForEditAsync(string id);
        Task<List<EmployeeView>> GetEmployeesAsync(string? managerId);
        Task<List<EmployeeView>> GetAllManagersAsync();
        Task AddEmployeeWithRolesAsync(EmployeeCreate employee);
        Task UpdateEmployeeWithRolesAsync(EmployeeEdit employee);
        Task DeleteAsync(string id);
    }
}
