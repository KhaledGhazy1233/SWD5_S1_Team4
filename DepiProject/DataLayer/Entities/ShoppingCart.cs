using System;

namespace DataLayer.Entities
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Navigation properties
        public ApplicationUser ApplicationUser { get; set; }
        public Product Product { get; set; }
    }
}
