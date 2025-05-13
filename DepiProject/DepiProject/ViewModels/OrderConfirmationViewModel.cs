using System;
using System.Collections.Generic;

namespace DepiProject.ViewModels
{
    public class OrderConfirmationViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShippingAddress { get; set; }
        public string PaymentMethod { get; set; }
        public string CustomerName { get; set; }
        public string TrackingNumber { get; set; }
        public string OrderStatus { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItemViewModel> Items { get; set; } = new List<OrderItemViewModel>();
    }

    public class OrderItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}
