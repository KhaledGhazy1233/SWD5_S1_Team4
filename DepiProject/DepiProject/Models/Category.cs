namespace DepiProject.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        // Add a property for subcategories if needed in the future
        public string? SubCategory { get; set; }
    }
}
