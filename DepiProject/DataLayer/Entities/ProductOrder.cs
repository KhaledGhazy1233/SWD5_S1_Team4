namespace DEPI_Project.Infrastructure.Entities;

public class ProductOrder
{
    public ProductOrder()
    {
        Product = new Product();
        Order = new Order();

        Name = string.Empty;
        Brand = string.Empty;
    }
    public int ProductOrderId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Amount { get; set; }
    public string Brand { get; set; }

    public int OrderId { get; set; }
    public int ProductId { get; set; }

    public Order Order { get; set; }
    public Product Product { get; set; }
}