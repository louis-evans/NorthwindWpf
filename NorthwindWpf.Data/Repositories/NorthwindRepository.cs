using Northwind.Data;
using System;

namespace NorthwindWpf.Data.Repositories
{
    public abstract class NorthwindRepository : IDisposable
    {
        protected NorthwindEntities NorthwindContext { get; }

        public NorthwindRepository()
        {
            NorthwindContext = new NorthwindEntities();
        }

        public void Dispose()
        {
            NorthwindContext.Dispose();
        }
    }
}
