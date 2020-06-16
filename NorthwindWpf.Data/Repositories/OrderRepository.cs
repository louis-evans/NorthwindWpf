using Northwind.Data;
using System;
using System.Linq;

namespace NorthwindWpf.Data.Repositories
{
    public class OrderRepository : IDisposable
    {
        private readonly NorthwindEntities _ctx;

        public OrderRepository()
        {
            _ctx = new NorthwindEntities();
        }

        public IQueryable<Order> GetAll() => _ctx.Orders;

        public Order GetById(int id) => _ctx.Orders.Find(id);

        public void Dispose()
        {
            _ctx.Dispose();
        }
    }
}
