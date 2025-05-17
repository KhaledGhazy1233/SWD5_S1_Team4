using System;
using System.Linq.Expressions;
using DataLayer.Context;
using DataLayer.Entities;
using DataLayer.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repository;

public class OrderRepository : IRepository<Order>, IOrderRepository
{
    private readonly ApplicationDbContext _db;
    private static readonly char[] _propertySeparators = new[] { ',' };
    public OrderRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    public void Add(Order entity)
    {
        _db.Orders.Add(entity);
    }
    public Order Get(Expression<Func<Order, bool>> filter, string? includeProperties = null)
    {
        IQueryable<Order> query = _db.Orders;

        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties.Split(_propertySeparators, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }

        return query.FirstOrDefault(filter);
    }

    public IEnumerable<Order> GetAll(Expression<Func<Order, bool>>? filter = null, string? includeProperties = null)
    {
        IQueryable<Order> query = _db.Orders;
        if (filter != null)
        {
            query = query.Where(filter);
        }
        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties.Split(_propertySeparators, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }
        return query.ToList();
    }

    public IEnumerable<Order> GetAll()
    {
        return _db.Orders.ToList();
    }

    public void Remove(Order entity)
    {
        _db.Orders.Remove(entity);
    }

    public void RemoveRange(IEnumerable<Order> entites)
    {
        _db.Orders.RemoveRange(entites);
    }

    public Task Update(Order entity)
    {
        _db.Orders.Update(entity);
        return Task.CompletedTask;
    }

    void IOrderRepository.Update(Order order)
    {
        _db.Orders.Update(order);
    }
}
