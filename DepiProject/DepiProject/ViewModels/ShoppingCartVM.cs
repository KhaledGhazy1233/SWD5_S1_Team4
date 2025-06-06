using DataLayer.Entities;
using DepiProject.Models;

namespace DepiProject.ViewModels
{
    public class ShoppingCartVM
    {
        public IEnumerable<DataLayer.Entities.ShoppingCart> ShoppingCartList { get; set; } = new List<DataLayer.Entities.ShoppingCart>();
        public double OrderTotal { get; set; }
        public OrderHeader? OrderHeader { get; set; }
    }
}
