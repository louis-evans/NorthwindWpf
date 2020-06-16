using Northwind.Data;
using System;
using System.Linq;

namespace NorthwindWpf.Data.Repositories
{
    public class CustomerRepository : NorthwindRepository
    {
        public IQueryable<Customer> GetAll() => NorthwindContext.Customers;

        public Customer GetById(int id) => NorthwindContext.Customers.Find(id);
    }
}
