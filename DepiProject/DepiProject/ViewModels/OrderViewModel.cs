using System;

namespace DepiProject.ViewModels
{
    public class OrderViewModel
    {
        public string Id { get; set; }
        public int RawId { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public int Items { get; set; }
        public decimal Total { get; set; }
    }
}
