using Northwind.Data;
using System;
using System.Linq;

namespace NorthwindWpf.Data.Repositories
{
    public class ProductRepository : IDisposable
    {
        private readonly NorthwindEntities _ctx;

        public ProductRepository()
        {
            _ctx = new NorthwindEntities();
        }

        public IQueryable<Product> GetAll() => _ctx.Products;

        public Product GetById(int id) => _ctx.Products.Find(id);

        public void Dispose()
        {
            _ctx.Dispose();
        }
    }
}
