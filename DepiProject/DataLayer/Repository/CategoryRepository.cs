using DataLayer.Context;
using DataLayer.Entities;
using DataLayer.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _db.Categories.Where(c => !c.IsDeleted).ToListAsync();
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
                category.IsDeleted = true;
                await SaveChangesAsync();
            }
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
