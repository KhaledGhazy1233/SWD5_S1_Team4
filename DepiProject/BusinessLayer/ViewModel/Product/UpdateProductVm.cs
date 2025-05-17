using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.ViewModel.Product;

public class UpdateProductVm
{
    public int? ProductId { get; set; }
    [Required] public string Name { get; set; }
    [Required] public string? Description { get; set; }
    [Required] public decimal Price { get; set; }
    public IFormFile? ImageUrl { get; set; }
    [Required] public int CategoryId { get; set; }
    [Required] public string? Brand { get; set; }
    [Required] public string? Model { get; set; }
    [Required] public string? TechnicalSpecifications { get; set; }
    [Required] public decimal DiscountPercentage { get; set; }
    [Required] public bool IsFeatured { get; set; }
    [Required] public string? WarrantyInfo { get; set; }
    [Required] public int StockQuantity { get; set; }
}