using DataLayer.Context;
using DataLayer.Entities;
using DataLayer.Repository.IRepository;

namespace DataLayer.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository

    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product product)
        {
            _db.Products.Update(product);
        }
    }
}