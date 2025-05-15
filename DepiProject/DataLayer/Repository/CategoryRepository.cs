using DataLayer.Context;
using DataLayer.Entities;
using DataLayer.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<bool> IsCategoryNameExist(string categoryName)
        {
            return await _db.Categories.AnyAsync(c => c.Name == categoryName && !c.IsDeleted);
        }
        public async Task<bool> IsCategoryNameExistExcludeItself(string categoryName, int categoryId)
        {
            return await _db.Categories.AnyAsync(c => c.Name == categoryName && !c.IsDeleted && c.CategoryId != categoryId);
        }
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _db.Categories.Where(c => !c.IsDeleted).ToListAsync();
        }
        public async Task<List<Category>> GetAllCategoriesIncludeProductsAsync()
        {
            return await _db.Categories.Include(c => c.Products).Where(c => !c.IsDeleted).ToListAsync();
        }
        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _db.Categories.FirstOrDefaultAsync(c => c.CategoryId == id && !c.IsDeleted);
        }

        public async Task SoftDeleteAsync(int id)
        {
            var category = await _db.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
            if (category != null)
            {
                category.DeletedAt = DateTime.UtcNow;
                category.IsDeleted = true;
                await SaveChangesAsync();
            }
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public IQueryable<Category> GetQuerableCategories()
        {
            return _db.Categories.Where(c => !c.IsDeleted);
        }
    }
}
