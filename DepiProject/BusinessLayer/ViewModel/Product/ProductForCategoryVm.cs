namespace BusinessLayer.ViewModel.Product;

public class ProductForCategoryVm
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string Brand { get; set; }
    public string CategoryName { get; set; }
    public string ImageUrl { get; set; }
    public decimal DiscountPercentage { get; set; }
}