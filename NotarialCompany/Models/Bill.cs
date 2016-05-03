using System;

namespace NotarialCompany.Models
{
    public class Bill
    {
        public int Id { get; set; }
        public decimal BasePrice { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsPaid { get; set; }
        public DateTime DateTime { get; set; }
    }
}
