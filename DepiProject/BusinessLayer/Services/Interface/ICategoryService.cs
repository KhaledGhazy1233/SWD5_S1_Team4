using BusinessLayer.ViewModel.Category;

namespace BusinessLayer.Services.Interface;

public interface ICategoryService
{
    public Task<string> Create(CreateCategoryVm vm);
}