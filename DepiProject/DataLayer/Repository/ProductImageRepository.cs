using DataLayer.Context;
using DataLayer.Entities;
using DataLayer.Repository.IRepository;

namespace DataLayer.Repository;

public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
{
    #region      Fields
    private ApplicationDbContext _db;
    #endregion

    #region      Constructor
    public ProductImageRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }
    #endregion

    #region      Methods
    #endregion
}