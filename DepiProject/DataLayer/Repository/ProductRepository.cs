using DataLayer.Context;
using DataLayer.Entities;
using DataLayer.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

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
    public async Task<bool> IsProductNameExist(string productName)
        {
        var exist = await _db.Products.AnyAsync(p => p.Name == productName);
        return exist;
        }

    public async Task<bool> IsProductNameExistExcludeItself(string productName, int productId)
    {
        var exist = await _db.Products.AnyAsync(p => p.Name == productName && p.ProductId != productId);
        return exist;
    }
    #endregion
} 