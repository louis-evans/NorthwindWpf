using System.Data.Entity;
using System.Linq;
using System.Windows;
using NorthwindWpf.ViewModels;
using NorthwindWpf.Data.Repositories;
using WpfApp1.Views;
using System.Threading.Tasks;

namespace NorthwindWpf.Views
{
    public partial class ViewOrderWindow : Window
    {
        private const string DATE_FORMAT = "dd/MM/yyyy";

        public int OrderId { get; private set; }

        public ViewOrderWindow(int orderId)
        {
            InitializeComponent();
            OrderId = orderId;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadOrder();
        }

        private async void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var window = new OrderEntryWindow
            {
                OrderId = OrderId
            };

            if (window.ShowDialog() == true)
            {
                await LoadOrder();
            }
        }

        private async Task LoadOrder()
        {
            using (var orderRepo = new OrderRepository())
            {
                var order = await orderRepo.GetByIdAsync(OrderId);

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
            }

            float calculateFinalPrice(decimal unitPrice, int qty, float discount)
            {
                var netTotal = unitPrice * qty;
                var discountTotal = (float)netTotal * discount;
                return (float)netTotal - discountTotal;
            }
        }
    }
}