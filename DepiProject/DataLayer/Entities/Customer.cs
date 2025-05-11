using System.Collections.Generic;

namespace DataLayer.Entities
{
    public class Customer
    {
        public Customer()
        {
            Orders = new List<Order>();
        }
        
        public int CustomerId { get; set; }
        public string ApplicationUserId { get; set; }
        
        // Navigation properties
        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}