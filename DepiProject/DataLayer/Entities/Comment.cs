using System;

namespace DataLayer.Entities
{
    public class Comment
    {
        public Comment()
        {
            Message = string.Empty;
            Email = string.Empty;
        }
        
        public int CommentId { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Foreign keys
        public int ProductId { get; set; }
        public string ApplicationUserId { get; set; }
        
        // Navigation properties
        public Product Product { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}