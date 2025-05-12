using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Orders = new List<Order>();
        }
        
        public DateTime Created { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = false;
        public string ?FirstName { get; set; }
        public string ?LastName { get; set; }
        public string ?City { get; set; }
        public string ?Country { get; set; }
        public string ?PhoneNumber { get; set; }
        // Navigation properties
        public virtual User? User { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
