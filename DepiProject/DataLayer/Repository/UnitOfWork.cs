using DataLayer.Context;
using DataLayer.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IProductRepository Product { get; private set; }
        public IUserRepository User { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IOrderDetailsRepository OrderDetails { get; private set; }
        public UnitOfWork(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            User = new UserRepository(_db, userManager);
            Product = new ProductRepository(_db);
            OrderDetails = new OrderDetailsRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);
            User = new UserRepository(_db, userManager);
            ShoppingCart = new ShoppingCartRepository(_db);
        }
            
        
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
