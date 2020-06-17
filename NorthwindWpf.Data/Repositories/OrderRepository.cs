using Northwind.Data;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using System;
using System.Data.Entity.Migrations;

namespace NorthwindWpf.Data.Repositories
{
    public interface IOrderRepository : IDisposable
    {
        IQueryable<Order> GetAll();
        Order GetById(int id);
        Task<Order> GetByIdAsync(int id);
        void SaveOrder(Order order);
    }

    public class OrderRepository : NorthwindRepository, IOrderRepository
    {
        public IQueryable<Order> GetAll() => OrderQueryBase();

        public Order GetById(int id) => OrderQueryBase().Single(o => o.OrderID == id);

        public async Task<Order> GetByIdAsync(int id) => await OrderQueryBase().SingleAsync(o => o.OrderID == id);

        private IQueryable<Order> OrderQueryBase()
        {
            return NorthwindContext.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.Shipper)
                    .Include(o => o.Order_Details)
                    .Include(o => o.Order_Details.Select(d => d.Product));
        }

        public void SaveOrder(Order order)
        {
            NorthwindContext.Orders.AddOrUpdate(order);
            NorthwindContext.SaveChanges();
        }
    }
}
