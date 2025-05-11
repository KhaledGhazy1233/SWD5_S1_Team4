using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public class Order
    {
        public Order()
        {
            ProductOrders = new List<ProductOrder>();
            AddressDelivered = string.Empty;
        }
        
        public int OrderId { get; set; }
        public int NumberOfProduct { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Fax { get; set; }
        public decimal FinalPrice { get; set; }
        public string AddressDelivered { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }
        
        // Shipping details
        public string PhoneNumber { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Name { get; set; }
        
        // Relations
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<ProductOrder> ProductOrders { get; set; }
    }
}