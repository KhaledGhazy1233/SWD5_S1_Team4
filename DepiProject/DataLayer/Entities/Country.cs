namespace DEPI_Project.Infrastructure.Entities;

public class Country
{
    public Country()
    {
        CountryName = string.Empty;
    }
    public int CountryId { get; set; }
    public string CountryName { get; set; }
}