namespace BusinessLayer.ViewModel.Product;

public class GetProductDetails
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string Brand { get; set; }
    public string? Model { get; set; }
    public string? TechnicalSpecifications { get; set; }
    public decimal DiscountPercentage { get; set; }
    public string? WarrantyInfo { get; set; }
    public string? ImageUrl { get; set; }
}