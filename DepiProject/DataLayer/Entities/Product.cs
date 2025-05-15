namespace DataLayer.Entities;

public class Product
{
    public Product()
    {
        ProductImages = new List<ProductImage>();
        ProductOrders = new List<ProductOrder>();
    }

    public int ProductId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsAvailable { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string Brand { get; set; }
    public string? Model { get; set; }
    public string? TechnicalSpecifications { get; set; }
    public decimal DiscountPercentage { get; set; }
    public bool IsFeatured { get; set; }
    public string? WarrantyInfo { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; } = false;

    // Relations
    public int CategoryId { get; set; }

    public Category Category { get; set; }
    public ICollection<ProductImage> ProductImages { get; set; }
    public ICollection<ProductOrder> ProductOrders { get; set; }
}