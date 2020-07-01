using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WpfApp1.Views;

namespace NorthwindWpf.Views
{
    public partial class MainWindow : Window
    {
        private readonly ICollection<Window> _openWindows;

        public MainWindow()
        {
            InitializeComponent();
            _openWindows = new List<Window>();
        }

        private void BtnViewOrders_Click(object sender, RoutedEventArgs e)
        {
            OpenWindow<ViewOrdersWindow>(singleWindow:true);
        }

        private void BtnNewOrder_Click(object sender, RoutedEventArgs e)
        {
            OpenWindow<OrderEntryWindow>();
        }

        private void BtnProducts_Click(object sender, RoutedEventArgs e)
        {
            OpenWindow<ViewProductsWindow>(singleWindow: true);
        }

        private void OpenWindow<T>(bool singleWindow = false) where T : Window, new()
        {
            if (singleWindow)
            {
                if (_openWindows.Any(w => w.GetType() == typeof(T)))
                {
                    _openWindows.First(w => w.GetType() == typeof(T)).Focus();
                    return;
                }
            }
            
            var window = new T();
            window.Closed += (sender, e) => _openWindows.Remove((Window)sender);

            _openWindows.Add(window);

            window.Show();
        }
    }
}
