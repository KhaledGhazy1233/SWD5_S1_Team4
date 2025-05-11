using DataLayer.Context;
using DataLayer.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public class UnitOfWork
    {
        private ApplicationDbContext? _db;
        public IShoppingCartRepository ShoppingCart { get; set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            ShoppingCart = new ShoppingCartRepository(_db);
          
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
