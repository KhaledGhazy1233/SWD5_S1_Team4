using DataLayer.Entities;

namespace DataLayer.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<bool> IsCategoryNameExist(string categoryName);
        public Task<bool> IsCategoryNameExistExcludeItself(string categoryName, int categoryId);

        Task<List<Category>> GetAllCategoriesAsync();
        public Task<List<Category>> GetAllCategoriesIncludeProductsAsync();
        public IQueryable<Category> GetQuerableCategories();

        Task<Category> GetCategoryByIdAsync(int id);
        Task SoftDeleteAsync(int id);
        Task SaveChangesAsync();
    }
}