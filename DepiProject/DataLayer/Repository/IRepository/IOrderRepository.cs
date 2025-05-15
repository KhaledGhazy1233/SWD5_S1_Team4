using System;
using DataLayer.Entities;

namespace DataLayer.Repository.IRepository;

public interface IOrderRepository : IRepository<Order>
{
    void Update(Order order);
}
