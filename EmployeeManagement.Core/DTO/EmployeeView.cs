namespace EmployeeManagement.Core.DTO
{
    public class EmployeeView
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string[] Roles { get; set; } = new string[] { };
    }
}
