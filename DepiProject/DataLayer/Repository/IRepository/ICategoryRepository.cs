using DataLayer.Entities;

namespace DataLayer.Repository.IRepository;

public interface ICategoryRepository : IRepository<Category>
{
    public Task<bool> IsCategoryNameExist(string categoryName);
}