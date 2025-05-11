namespace DataLayer.Entities
{
    public class ProductImage
    {
        public ProductImage()
        {
            Path = string.Empty;
        }
        
        public int ProductImageId { get; set; }
        public string Path { get; set; }
        
        // Foreign key
        public int ProductId { get; set; }
        
        // Navigation property
        public Product Product { get; set; }
    }
}