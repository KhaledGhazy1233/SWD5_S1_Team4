using DataLayer.Entities;

namespace DataLayer.Repository.IRepository;

    public interface IProductRepository : IRepository<Product>
    {
    public Task<bool> IsProductNameExist(string productName);
    public Task<bool> IsProductNameExistExcludeItself(string productName, int productId);
} 