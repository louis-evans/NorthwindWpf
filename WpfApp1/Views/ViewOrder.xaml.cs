using SecPlus;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
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