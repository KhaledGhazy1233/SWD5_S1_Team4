using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.ViewModel.Product;

public class CreateProductVm
{
    [Required] public string Name { get; set; } = string.Empty;
    [Required] public string Description { get; set; } = string.Empty;
    [Required] public decimal Price { get; set; }
    [Required] public int Amount { get; set; }
    [Required] public string Brand { get; set; } = string.Empty;
    [Required] public int CategoryId { get; set; }
    [Required] public List<IFormFile> Files { get; set; }
}