using DepiProject.Models;

namespace DepiProject.ViewModel
{
    public class ShoppingCartVM
    {
        public IEnumerable<CartItem> ? ShoppingCart { get; set; }
        public double TotalPrice { get; set; }
    }
}
