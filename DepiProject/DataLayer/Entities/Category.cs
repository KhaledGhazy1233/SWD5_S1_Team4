namespace DataLayer.Entities
{
    public class Category
    {
        public Category()
        {
            Products = new List<Product>();
        }

        //  public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; } = false;

        // Navigation properties
        public ICollection<Product> Products { get; set; }
    }
}