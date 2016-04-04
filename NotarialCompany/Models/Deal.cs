using System;
using System.Collections.Generic;

namespace NotarialCompany.Models
{
    public class Deal
    {
        public int Id { get; set; }
        public decimal Bill { get; set; }
        public bool IsPaid {get; set; }
        public DateTime Date { set; get; }
        public string Description { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public ICollection<Services> Services { get; set; }
    }
}
