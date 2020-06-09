using SecPlus;
using System;
using System.Windows;

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
