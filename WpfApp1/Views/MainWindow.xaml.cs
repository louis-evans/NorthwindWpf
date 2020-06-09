using SecPlus;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace NorthwindWpf.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly NorthwindEntities _ctx;

        public MainWindow()
        {
            InitializeComponent();

            _ctx = new NorthwindEntities();

            //CmbCustomers.DisplayMemberPath = "CompanyName";
            //CmbCustomers.SelectedValuePath = "CustomerID";
            //CmbCustomers.ItemsSource = _ctx.Customers.Select(c => new { c.CompanyName, c.CustomerID }).ToArray();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _ctx?.Dispose();
        }

        private void BtnViewOrders_Click(object sender, RoutedEventArgs e)
        {
            new ViewOrders().ShowDialog();
        }

        private void BtnNewOrder_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
