using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{      
    public class User
    {
        public User()
        {
            Orders = new List<Order>();
            FirstName = string.Empty;
            LastName = string.Empty;
            Address = string.Empty;
            City = string.Empty;
            State = string.Empty;
            PostalCode = string.Empty;
            Phone = string.Empty;
        }
        
        public string Id { get; set; } // Maps to ApplicationUser Id - Primary key and foreign key
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}