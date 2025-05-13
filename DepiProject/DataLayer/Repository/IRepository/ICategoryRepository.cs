using DataLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<bool> IsCategoryNameExist(string categoryName);
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int id);
        Task SoftDeleteAsync(int id);
        Task SaveChangesAsync();
    }
}