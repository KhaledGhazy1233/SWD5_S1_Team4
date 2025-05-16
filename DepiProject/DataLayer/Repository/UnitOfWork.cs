using DataLayer.Context;
using DataLayer.Entities;
using DataLayer.Repository.IRepository;
using Microsoft.AspNetCore.Identity;

namespace DataLayer.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db; public IShoppingCartRepository ShoppingCart { get; private set; } = null!;
        public IProductRepository Product { get; private set; } = null!;
        public IUserRepository User { get; private set; } = null!;
        public IOrderHeaderRepository OrderHeader { get; private set; } = null!;
        public IOrderDetailsRepository OrderDetails { get; private set; } = null!;
        public IOrderRepository Orders { get; private set; } = null!;
        public ICategoryRepository Category { get; private set; } = null!;

        // This appears to be a duplicate but is required by the interface
        public object Order => null!; public UnitOfWork(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            User = new UserRepository(_db, userManager);
            Product = new ProductRepository(_db);
            OrderDetails = new OrderDetailsRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);
            Orders = new IRepository.OrderRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
            Category = new CategoryRepository(_db);
        }


        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
