using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Data
{
    public partial class Customer
    {
        public override bool Equals(object obj)
        {
            return ((Customer)obj).CustomerID == CustomerID;
        }

        public override int GetHashCode()
        {
            return CustomerID.GetHashCode();
        }
    }
}
