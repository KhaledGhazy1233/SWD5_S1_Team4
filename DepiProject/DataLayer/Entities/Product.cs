namespace DEPI_Project.Infrastructure.Entities;

public class Product
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Amount { get; set; }
    public string Brand { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime DeletedAt { get; set; }
    public bool IsDeleted { get; set; } = false;


    public int CountryId { get; set; }
    public int CategoryId { get; set; }


    public Country Country { get; set; }
    public Category Category { get; set; }
}