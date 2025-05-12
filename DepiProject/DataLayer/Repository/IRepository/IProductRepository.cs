using DataLayer.Entities;

namespace DataLayer.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product product);
    }
} 