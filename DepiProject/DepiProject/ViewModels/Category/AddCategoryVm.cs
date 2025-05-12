using System.ComponentModel.DataAnnotations;

namespace DepiProject.ViewModels.Category;

public class AddCategoryVm
{
    [Required]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public string Description { get; set; } = string.Empty;
}
