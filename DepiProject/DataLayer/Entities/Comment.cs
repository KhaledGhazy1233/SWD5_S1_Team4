namespace DEPI_Project.Infrastructure.Entities;

public class Comment
{
    public Comment()
    {
        Massage = string.Empty;
        Email = string.Empty;
    }
    public string Massage { get; set; }
    public string Email { get; set; }
}