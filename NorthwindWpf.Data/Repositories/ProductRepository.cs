using Northwind.Data;
using System;
using System.Data.Entity;
using System.Linq;

namespace NorthwindWpf.Data.Repositories
{
    public interface IProductRepository : IDisposable
    {
        IQueryable<Product> GetAll();
        Product GetById(int id);
    }

    public class ProductRepository : NorthwindRepository, IProductRepository
    {
        public IQueryable<Product> GetAll() => NorthwindContext.Products.Include(x => x.Category).Include(x => x.Supplier);

        public Product GetById(int id) => NorthwindContext.Products.Find(id);
    }
}
