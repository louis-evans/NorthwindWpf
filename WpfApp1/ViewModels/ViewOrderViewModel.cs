using System.Collections.Generic;
using WpfApp1.ViewModels;

namespace NorthwindWpf.ViewModels
{
    public class ViewOrderViewModel : ViewModelBase
    {
        public int OrderID { get; internal set; }
        public string CustomerName { get; internal set; }
        public string OrderDate { get; internal set; }
        public string RequiredDate { get; internal set; }
        public string ShipMethod { get; internal set; }
        public string ShipDate { get; internal set; }

        public IEnumerable<LineItemModel> LineItems { get; set; }

        public class LineItemModel
        {
            public string ProductName { get; internal set; }
            public decimal UnitPrice { get; internal set; }
            public int Qty { get; internal set; }
            public float Discount { get; internal set; }
            public decimal TotalPrice { get; internal set; }
            public string UnitPriceDisplay => UnitPrice.ToString("N2");
            public string DiscountDisplay => (Discount * 100).ToString("0.##");
            public string TotalPriceDisplay => TotalPrice.ToString("N2");
        }
    }
}
