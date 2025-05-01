namespace DEPI_Project.Infrastructure.Entities;

public class Order
{
    public Order()
    {
        Customer = new Customer();
        AddressDelivered = string.Empty;
    }
    public int OrderId { get; set; }
    public int NumberOfProduct { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal Fax { get; set; }
    public decimal FinalPrice { get; set; }
    public string AddressDelivered { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }

    public int CustomerId { get; set; }

    public Customer Customer { get; set; }
}