using Northwind.Data;
using System;
using System.Linq;

namespace NorthwindWpf.Data.Repositories
{
    public class CustomerRepository : IDisposable
    {
        private readonly NorthwindEntities _ctx;

        public CustomerRepository()
        {
            _ctx = new NorthwindEntities();
        }

        public IQueryable<Customer> GetAll() => _ctx.Customers;

        public Customer GetById(int id) => _ctx.Customers.Find(id);

        public void Dispose()
        {
            _ctx.Dispose();
        }
    }
}
