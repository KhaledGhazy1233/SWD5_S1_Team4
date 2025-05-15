using BusinessLayer.ViewModel.Category;
using DataLayer.Entities;

namespace BusinessLayer.Services.Interface
{
    public interface ICategoryService
    {
        Task<List<ShowAllCategoryVm>> GetPaginatedCategories(int pageSize, int pageNumber);
        List<CategoryDropDown> GetDropDown();
        Task<List<ShowAllCategoryVm>> GetAllCategories();
        Task<Category> GetCategoryById(int id);
        Task<UpdateCategoryVm?> GetUpdateCategoryVmById(int id);
        Task<string> Create(CreateCategoryVm vm);
        Task<string> Update(UpdateCategoryVm vm);
        Task<string> Delete(int id);
    }
}
