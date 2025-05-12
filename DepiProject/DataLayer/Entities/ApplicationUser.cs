using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace DataLayer.Entities;

public class ApplicationUser : IdentityUser
{
    public ApplicationUser()
    {
        Orders = new List<Order>();
    }
    
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public DateTime Created { get; set; } = DateTime.Now;
    public bool IsActive { get; set; } = false;
    
    // Navigation properties
    public virtual ICollection<Order> Orders { get; set; }
}
