namespace DEPI_Project.Infrastructure.Entities;

public class Category
{
    public Category()
    {
        CreatedBy = new User();
        DeletedBy = new User();

        Name = string.Empty;
    }
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime DeletedAt { get; set; }
    public bool IsDeleted { get; set; }

    public int CategoryTypeId { get; set; }
    public int CreatedById { get; set; }
    public int DeletedById { get; set; }


    public CategoryType CategoryType { get; set; }
    public User CreatedBy { get; set; }
    public User DeletedBy { get; set; }
}