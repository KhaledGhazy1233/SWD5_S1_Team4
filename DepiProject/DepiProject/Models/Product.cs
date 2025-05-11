namespace DepiProject.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public string? ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        // Added for electronics store
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? TechnicalSpecifications { get; set; }
        public decimal DiscountPercentage { get; set; }
        public bool IsFeatured { get; set; }
        public string? WarrantyInfo { get; set; }
        public int StockQuantity { get; set; }
    }
}
