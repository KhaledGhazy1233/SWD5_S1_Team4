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
        
        // Navigation properties
        public virtual User? User { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
