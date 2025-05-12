using DataLayer.Context;
using DataLayer.Entities;
using DataLayer.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repository;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    #region      Fields
    private ApplicationDbContext _db;
    #endregion

    #region      Constructor
    public CategoryRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }
    #endregion

    #region      Methods
    public async Task<bool> IsCategoryNameExist(string categoryName)
    {
        var exist = await _db.Categories.AnyAsync(c => c.Name == categoryName);
        return exist;
    }
    #endregion
}