using System;

namespace NotarialCompany.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime EmploymentDate { get; set; }
        public int EmployeesPositionId { get; set; }
        public EmployeesPosition EmployeesPosition { get; set; }
    }
}
