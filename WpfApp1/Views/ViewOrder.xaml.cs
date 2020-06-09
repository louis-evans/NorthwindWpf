using SecPlus;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using NorthwindWpf.ViewModels;

namespace NorthwindWpf.Views
{
    /// <summary>
    /// Interaction logic for ViewOrder.xaml
    /// </summary>
    public partial class ViewOrder : Window
    {
        public int OrderId { get; private set; }

        private readonly NorthwindEntities _ctx;

        public ViewOrder(int orderId)
        {
            InitializeComponent();
            OrderId = orderId;

            _ctx = new NorthwindEntities();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _ctx.Dispose();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var orderTask = _ctx.Orders
                .Include(o => o.Customer)
                .Include(o => o.Shipper)
                .SingleAsync(o => o.OrderID == OrderId);

            var lineItemTask = _ctx.Order_Details
                .Include(x => x.Product)
                .Where(x => x.OrderID == OrderId)
                .ToArrayAsync();

            await Task.WhenAll(orderTask, lineItemTask);


            DataContext = new ViewOrderViewModel
            {
                OrderID = orderTask.Result.OrderID,
                CustomerName = orderTask.Result.Customer.CompanyName,
                OrderDate = orderTask.Result.OrderDate?.ToString("dd/MM/yyyy") ?? "",
                RequiredDate = orderTask.Result.RequiredDate?.ToString("dd/MM/yyyy") ?? "",
                ShipMethod = orderTask.Result.Shipper.CompanyName,
                ShipDate = orderTask.Result.ShippedDate?.ToString("dd/MM/yyyy") ?? "",
                LineItems = lineItemTask.Result.Select(x => new ViewOrderViewModel.LineItemModel
                {
                    ProductName = x.Product.ProductName,
                    UnitPrice = x.UnitPrice,
                    Qty = x.Quantity,
                    Discount = x.Discount,
                    TotalPrice = calculateFinalPrice(x.UnitPrice, x.Quantity, x.Discount)
                })
                
            };

            float calculateFinalPrice(decimal unitPrice, int qty, float discount)
            {
                var netTotal = unitPrice * qty;
                var discountTotal = (float)netTotal * discount;
                return (float)netTotal - discountTotal;
            }
        }
    }
}

namespace NorthwindWpf.ViewModels
{
    public class ViewOrderViewModel
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
            public float TotalPrice { get; internal set; }
            public string UnitPriceDisplay => UnitPrice.ToString("N2");
            public string DiscountDisplay => (Discount * 100).ToString("0.##");
            public string TotalPriceDisplay => TotalPrice.ToString("N2");
        }
    }
}
