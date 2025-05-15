using BusinessLayer.ViewModel.Category;

namespace BusinessLayer.ViewModel.Product;

public class GetProductEditVm
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Amount { get; set; }
    public string Brand { get; set; }
    public string CategoryName { get; set; }
    public List<CategoryDropDown> categoryDropDowns { get; set; }
}