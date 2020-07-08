namespace NorthwindWpf.Core.Utils
{
    public static class OrderUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitPrice"></param>
        /// <param name="qty"></param>
        /// <param name="discount">Example: for 25% pass 25f NOT 0.25f</param>
        /// <returns></returns>
        public static decimal CalculateLineTotal(decimal unitPrice, int qty, float discount = default)
        {
            var netTotal = unitPrice * qty;

            if (discount == default) return netTotal;
            
            var discountTotal = netTotal * ((decimal)discount / 100);
            return netTotal - discountTotal;
        }
    }
}
