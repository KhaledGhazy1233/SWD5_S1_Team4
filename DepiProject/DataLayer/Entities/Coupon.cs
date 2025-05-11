using System;

namespace DataLayer.Entities
{
    public class Coupon
    {
        public Coupon()
        {
            CouponNumber = string.Empty;
            Code = string.Empty;
        }
        
        public int CouponId { get; set; }
        public string CouponNumber { get; set; }
        public string Code { get; set; }
        public decimal Discount { get; set; }
        public decimal MinimumAmount { get; set; }
        public int MaxUsage { get; set; }
        public int UsageCount { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ExpiryDate { get; set; }
    }
}