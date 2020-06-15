using SecPlus;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using NorthwindWpf.ViewModels;

namespace NorthwindWpf.Views
{
    public partial class ViewOrder : Window
    {
        private const string DATE_FORMAT = "dd/MM/yyyy";

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
            var order = await _ctx.Orders
                .Include(o => o.Customer)
                .Include(o => o.Shipper)
                .Include(o => o.Order_Details)
                .Include(o => o.Order_Details.Select(d => d.Product))
                .SingleAsync(o => o.OrderID == OrderId);

            DataContext = new ViewOrderViewModel
            {
                OrderID = order.OrderID,
                CustomerName = order.Customer.CompanyName,
                OrderDate = order.OrderDate?.ToString(DATE_FORMAT) ?? "",
                RequiredDate = order.RequiredDate?.ToString(DATE_FORMAT) ?? "",
                ShipMethod = order.Shipper.CompanyName,
                ShipDate = order.ShippedDate?.ToString(DATE_FORMAT) ?? "",
                LineItems = order.Order_Details.Select(x => new ViewOrderViewModel.LineItemModel
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