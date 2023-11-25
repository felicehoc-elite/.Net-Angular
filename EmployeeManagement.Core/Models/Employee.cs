using EmployeeManagement.Core.Helper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Core.Models
{
    [Table("Employees")]
    public class Employee
    {
        public Employee()
        {
            Roles = new List<EmployeeRole>();
        }

        public Employee(string? id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                Id = id;
            }
            Roles = new List<EmployeeRole>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; private set; } = IdGenerator.NewId();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ManagerId { get; set; }
        public ICollection<EmployeeRole> Roles { get; set; }

    }
}