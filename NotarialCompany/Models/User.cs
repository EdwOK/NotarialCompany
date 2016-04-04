using System;

namespace NotarialCompany.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public Employee Employee { get; set; }
        public int? EmployeeId { get; set; }
    }
}
