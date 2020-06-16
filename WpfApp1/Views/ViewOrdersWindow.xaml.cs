﻿using Northwind.Data;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NorthwindWpf.Views
{
    /// <summary>
    /// Interaction logic for ViewOrders.xaml
    /// </summary>
    public partial class ViewOrdersWindow : Window
    {
        private readonly NorthwindEntities _ctx;

        public ViewOrdersWindow()
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

                var viewOrder = new ViewOrderWindow(orderId);
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