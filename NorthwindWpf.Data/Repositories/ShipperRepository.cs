using Northwind.Data;
using System;
using System.Linq;

namespace NorthwindWpf.Data.Repositories
{
    public interface IShipperRepository : IDisposable
    {
        IQueryable<Shipper> GetAll();
        Shipper GetById(int id);
    }

    public class ShipperRepository : NorthwindRepository, IShipperRepository
    {
        public IQueryable<Shipper> GetAll() => NorthwindContext.Shippers;

        public Shipper GetById(int id) => NorthwindContext.Shippers.Find(id);
    }
}
