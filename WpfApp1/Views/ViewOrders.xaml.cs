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

namespace NorthwindWpf.Views
{
    /// <summary>
    /// Interaction logic for ViewOrders.xaml
    /// </summary>
    public partial class ViewOrders : Window
    {
        private readonly NorthwindEntities _ctx;

        public ViewOrders()
        {
            InitializeComponent();
            _ctx = new NorthwindEntities();           
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _ctx.Dispose();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadOrders();
        }

        private async void LstOrders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var row = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;

            if (row == null) return;

            try
            {
                var orderId = ((OrderLineModel)row.Item).OrderID;

                var viewOrder = new ViewOrder(orderId);
                var result = viewOrder.ShowDialog();

                if (result ?? false)
                {
                    await LoadOrders();
                }
            }
            catch
            {
                MessageBox.Show("Cannot open order");
            }
        }

        private async Task LoadOrders()
        {
            TxtLoading.Visibility = Visibility.Visible;
            LstOrders.Visibility = Visibility.Hidden;

            var orders = await _ctx.Orders.OrderByDescending(o => o.OrderDate).Select(o => new OrderLineModel
            {
                OrderID = o.OrderID,
                CompanyName = o.Customer.CompanyName,
                OrderDate = o.OrderDate,
                ItemCount = o.Order_Details.Count()
            })
           .ToListAsync();

            TxtLoading.Visibility = Visibility.Hidden;

            LstOrders.ItemsSource = orders;
            LstOrders.Visibility = Visibility.Visible;
        }

        private class OrderLineModel
        {
            public int OrderID { get; set; }
            public string CompanyName { get; set; }
            public DateTime? OrderDate { get; set; }
            public int ItemCount { get; set; }
        }
    }
}
