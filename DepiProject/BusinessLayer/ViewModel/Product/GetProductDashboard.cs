namespace BusinessLayer.ViewModel.Product;

public class GetProductDashboard
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string CategoryNAme { get; set; }
    public bool IsAvailable { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string ImageUrl { get; set; }
}