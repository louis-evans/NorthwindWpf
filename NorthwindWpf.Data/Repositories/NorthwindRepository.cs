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

        /// <summary>
        /// NOTE: If overriding, be sure to call base.Dispose()
        /// </summary>
        public virtual void Dispose()
        {
            NorthwindContext.Dispose();
        }
    }
}
