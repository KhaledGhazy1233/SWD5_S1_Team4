using BusinessLayer.ViewModel.Category;
using DataLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Interface
{
    public interface ICategoryService
    {
        Task<string> Create(CreateCategoryVm vm);
        Task<string> Update(UpdateCategoryVm vm);
        Task<string> Delete(int id);
        Task<List<Category>> GetAllCategories();
        Task<Category> GetCategoryById(int id);
       // last
       Task<UpdateCategoryVm?> GetUpdateCategoryVmById(int id);

    }
}
