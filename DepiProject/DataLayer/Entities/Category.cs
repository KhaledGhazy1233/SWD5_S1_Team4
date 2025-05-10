namespace DEPI_Project.Infrastructure.Entities;

public class Category
{
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}