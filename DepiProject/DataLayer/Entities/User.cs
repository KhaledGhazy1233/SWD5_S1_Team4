namespace DEPI_Project.Infrastructure.Entities;

public class User
{

    public int UserId { get; set; }
    public string NationalId { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public decimal Bonus { get; set; }

    public int PersonId { get; set; }
    public Person? Person { get; set; }
    public int CountryId { get; set; }
    public Country? Country { get; set; }
}