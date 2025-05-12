using DataLayer.Context;
using DataLayer.Entities;
using DataLayer.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private ApplicationDbContext _db;
        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(OrderHeader obj)
        {
            _db.Update(obj);
        }

        public void UpdateStatus(int id, string orderStatus, string PaymentStatus = null)
        {
            var orderfromDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);
            if (orderfromDb != null)
            {
                orderfromDb.OrderStatus = orderStatus;
                if (!string.IsNullOrEmpty(PaymentStatus))
                {
                    orderfromDb.PaymentStatus = PaymentStatus;
                }
            }
        }

        public void UpdateStripePaymentID(int id, string sessionId, string paymentIntendId)
        {
            var orderfromDb = _db.OrderHeaders.First(u => u.Id == id);


            if (!string.IsNullOrEmpty(sessionId))
            {
                orderfromDb.SessionId = sessionId;
            }
            if (!string.IsNullOrEmpty(paymentIntendId))
            {
                orderfromDb.PaymentIntentId = paymentIntendId;
                orderfromDb.PaymentDate = DateTime.Now;
            }
        }
    }
}

