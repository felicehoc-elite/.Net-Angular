using EmployeeManagement.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Core.DTO
{
    public class EmployeeCreate
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string? ManagerId { get; set; }
        [Required]
        public int[] RoleIds { get; set; } = Array.Empty<int>();

        public Employee Entity()
        {
            return new Employee { FirstName = FirstName, LastName = LastName, ManagerId = ManagerId };
        }
    }
}
