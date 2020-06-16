using Northwind.Data;
using System.Linq;

namespace NorthwindWpf.Data.Repositories
{
    public class ProductRepository : NorthwindRepository
    {
        public IQueryable<Product> GetAll() => NorthwindContext.Products;

        public Product GetById(int id) => NorthwindContext.Products.Find(id);
    }
}
