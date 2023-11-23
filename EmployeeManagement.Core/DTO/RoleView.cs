using EmployeeManagement.Core.Models;

namespace EmployeeManagement.Core.DTO
{
    public class RoleView
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Role Entity()
        {
            return new Role { Id = Id, Name = Name };
        }
    }
}
