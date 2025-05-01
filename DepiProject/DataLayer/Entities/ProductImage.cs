namespace DEPI_Project.Infrastructure.Entities;

public class ProductImage
{
    public ProductImage()
    {
        Product = new Product();

        Path = string.Empty;
    }
    public int ProductImageId { get; set; }
    public string Path { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
}