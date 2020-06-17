using Northwind.Data;
using System;
using System.Linq;

namespace NorthwindWpf.Data.Repositories
{
    public interface ICustomerRepository : IDisposable
    {
        IQueryable<Customer> GetAll();
        Customer GetById(int id);
    }

    public class CustomerRepository : NorthwindRepository, ICustomerRepository
    {
        public IQueryable<Customer> GetAll() => NorthwindContext.Customers;

        public Customer GetById(int id) => NorthwindContext.Customers.Find(id);
    }
}
