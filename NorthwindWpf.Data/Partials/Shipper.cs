namespace Northwind.Data
{
    public partial class Shipper
    {
        public override bool Equals(object obj)
        {
            return (obj as Shipper)?.ShipperID == ShipperID;
        }

        public override int GetHashCode()
        {
            return ShipperID.GetHashCode();
        }
    }
}
