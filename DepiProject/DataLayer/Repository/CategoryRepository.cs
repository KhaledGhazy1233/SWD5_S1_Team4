using DataLayer.Context;
using DataLayer.Entities;
using DataLayer.Repository.IRepository;

namespace DataLayer.Repository;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    private ApplicationDbContext _db;
    public CategoryRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }


}