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
        //public IShoppingCartRepository ShoppingCart { get; private set; }
        public IUserRepository User { get; private set; }
        public IProductRepository Product { get; private set; }
        
        public UnitOfWork(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            //ShoppingCart = new ShoppingCartRepository(_db);
            User = new UserRepository(_db, userManager);
            Product = new ProductRepository(_db);
        }
        
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
