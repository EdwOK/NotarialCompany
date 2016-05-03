using System;
using System.Collections.Generic;

namespace NotarialCompany.Models
{
    public class Deal
    {
        public int Id { get; set; }
        public Bill Bill { get; set; }
        public string Description { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public ICollection<int> ServiceIds { get; set; }
    }
}
