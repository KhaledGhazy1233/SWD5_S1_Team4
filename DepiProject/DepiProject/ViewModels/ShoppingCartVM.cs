using DepiProject.Models;

namespace DepiProject.ViewModels
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> ? ShoppingCart { get; set; }
        public double TotalPrice { get; set; }
    }
}
