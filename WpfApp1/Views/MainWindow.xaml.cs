using System.Windows;
using WpfApp1.Views;

namespace NorthwindWpf.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnViewOrders_Click(object sender, RoutedEventArgs e)
        {
            new ViewOrdersWindow().ShowDialog();
        }

        private void BtnNewOrder_Click(object sender, RoutedEventArgs e)
        {
            new OrderEntryWindow().ShowDialog();
        }
    }
}
