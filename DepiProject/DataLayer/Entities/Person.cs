namespace DEPI_Project.Infrastructure.Entities;

public class Person : IdentityUser
{
    public Person()
    {
        ImagePath = string.Empty;
        Address = string.Empty;
    }
    public string ImagePath { get; set; }
    public DateTime DeletedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string Address { get; set; }
    public bool IsDeleted { get; set; }
}