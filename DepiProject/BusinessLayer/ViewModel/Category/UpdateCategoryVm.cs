using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.ViewModel.Category;

public class UpdateCategoryVm
{

    [Required] public int Id { get; set; }
    [Required] public string Name { get; set; }
    [Required] public string Description { get; set; }
}


