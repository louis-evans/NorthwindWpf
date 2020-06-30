namespace NorthwindWpf.Core.Utils
{
    public static class OrderUtils
    {
        public static decimal CalculateLineTotal(decimal unitPrice, int qty, float discount = default)
        {
            var netTotal = unitPrice * qty;

            if (discount == default) return netTotal;
            
            var discountTotal = netTotal * ((decimal)discount / 100);
            return netTotal - discountTotal;
        }
    }
}
