using DataLayer.Context;
using DataLayer.Entities;
using DataLayer.Repository.IRepository;

namespace DataLayer.Repository;

public class ProductRepository : Repository<Product>, IProductRepository
{
    #region       Fields
    private ApplicationDbContext _db;

    #endregion

    #region     Constructor
    public ProductRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }
    #endregion

    #region      Methods
    #endregion
}