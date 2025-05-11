namespace DepiProject.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public double Price { get; set; }
        public int Count { get; set; }
        public string ? Image { get; set; }
    }
}
