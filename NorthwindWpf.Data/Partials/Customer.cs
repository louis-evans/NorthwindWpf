namespace Northwind.Data
{
    public partial class Customer
    {
        public override bool Equals(object obj)
        {
            return (obj as Customer)?.CustomerID == CustomerID;
        }

        public override int GetHashCode()
        {
            return CustomerID.GetHashCode();
        }
    }
}
