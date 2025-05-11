using System;

namespace DataLayer.Entities
{
    public class ProductOrder
    {
        public ProductOrder()
        {
            Name = string.Empty;
            Brand = string.Empty;
        }
        
        public int ProductOrderId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public string Brand { get; set; }

        // Foreign keys
        public int OrderId { get; set; }
        public int ProductId { get; set; }

        // Navigation properties
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}