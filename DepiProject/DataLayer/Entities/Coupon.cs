namespace DEPI_Project.Infrastructure.Entities;

public class Coupon
{
    public Coupon()
    {
        CouponNumber = string.Empty;
    }
    public string CouponNumber { get; set; }
    public decimal Discount { get; set; }
}