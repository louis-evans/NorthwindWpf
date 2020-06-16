using Northwind.Data;
using System;
using System.Linq;

namespace NorthwindWpf.Data.Repositories
{
    public class ShipperRepository : IDisposable
    {
        private readonly NorthwindEntities _ctx;

        public ShipperRepository()
        {
            _ctx = new NorthwindEntities();
        }

        public IQueryable<Shipper> GetAll() => _ctx.Shippers;

        public Shipper GetById(int id) => _ctx.Shippers.Find(id);

        public void Dispose()
        {
            _ctx.Dispose();
        }
    }
}
