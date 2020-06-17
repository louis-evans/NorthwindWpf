using Northwind.Data;
using System;
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
        public IQueryable<Product> GetAll() => NorthwindContext.Products;

        public Product GetById(int id) => NorthwindContext.Products.Find(id);
    }
}
