using BusinessLayer.Services.Interface;
using BusinessLayer.ViewModel.Category;
using DataLayer.Entities;
using DataLayer.Repository.IRepository;

namespace BusinessLayer.Services.Implementation;

public class CategoryService : ICategoryService
{
    #region   Fields
    private readonly ICategoryRepository _categoryRepository;
    #endregion

    #region   Constructor
    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    #endregion

    #region    Handle Methods
    public async Task<string> Create(CreateCategoryVm vm)
    {
        // check name not exist
        if (await _categoryRepository.IsCategoryNameExist(vm.Name))
            return "This name is already exist";

        // map to category
        var category = new Category()
        {
            Description = vm.Description,
            Name = vm.Name,
        };
        // then save
        _categoryRepository.Add(category);

        return "Success";
    }
    #endregion
}