using Northwind.Data;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;

namespace NorthwindWpf.Data.Repositories
{
    public class OrderRepository : NorthwindRepository
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
    }
}
