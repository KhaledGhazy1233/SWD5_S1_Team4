using System.Collections.Generic;

namespace DataLayer.Entities
{
    public class Country
    {
        public Country()
        {
            CountryName = string.Empty;
            Products = new List<Product>();
        }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        
        // Navigation properties
        public ICollection<Product> Products { get; set; }
    }
}