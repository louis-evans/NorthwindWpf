using Northwind.Data;
using System.Linq;

namespace NorthwindWpf.Data.Repositories
{
    public class ShipperRepository : NorthwindRepository
    {
        public IQueryable<Shipper> GetAll() => NorthwindContext.Shippers;

        public Shipper GetById(int id) => NorthwindContext.Shippers.Find(id);
    }
}
