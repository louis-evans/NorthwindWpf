namespace Northwind.Data
{
    public partial class Shipper
    {
        public override bool Equals(object obj)
        {
            return ((Shipper)obj).ShipperID == ShipperID;
        }

        public override int GetHashCode()
        {
            return ShipperID.GetHashCode();
        }
    }
}
