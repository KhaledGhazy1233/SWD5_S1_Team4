using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.ViewModel.Category;

public class CreateCategoryVm
{
    [Required] public string Name { get; set; }
    [Required] public string Description { get; set; }
    [Required] public IFormFile ImageUrl { get; set; }
}