using AutoMapper;
using NorthwindWpf.Core.Service;
using NorthwindWpf.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp1.Models;
using WpfApp1.ViewModels;

namespace NorthwindWpf.Views
{
    public partial class ViewOrdersWindow : Window
    {
        private readonly IDictionary<int, ViewOrderWindow> _openOrderWindows;
        private readonly IOrderRepository _orderRepo;
        private readonly ViewOrdersViewModel _viewModel;

        public ViewOrdersWindow()
        {
            InitializeComponent();
            _openOrderWindows = new Dictionary<int, ViewOrderWindow>();

            _orderRepo = ServiceResolver.Get().Resolve<IOrderRepository>();

            _viewModel = new ViewOrdersViewModel();
            DataContext = _viewModel;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadOrders();
        }

        private void LstOrders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var row = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;

            if (row == null) return;

            try
            {
                var orderId = ((OrderLineModel)row.Item).OrderID;

                if (_openOrderWindows.ContainsKey(orderId))
                {
                    _openOrderWindows[orderId].Focus();
                }
                else
                {
                    var viewOrder = new ViewOrderWindow(orderId);
                    viewOrder.Closed += OnWindowClosed;
                    _openOrderWindows.Add(orderId, viewOrder);
                    viewOrder.Show();
                }
            }
            catch
            {
                MessageBox.Show("Cannot open order");
            }
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            var window = sender as ViewOrderWindow;

            if (_openOrderWindows.ContainsKey(window.OrderId))
            {
                _openOrderWindows.Remove(window.OrderId);
            }          
        }

        private async Task LoadOrders()
        {
            SetLoadingState(true);
            
            var orders = _orderRepo.GetAll()
                .OrderByDescending(o => o.OrderDate)
                .ToArrayAsync();

            _viewModel.OrderLines = Mapper.Map<IEnumerable<OrderLineModel>>(await orders);

            SetLoadingState(false);
        }

        private void SetLoadingState(bool isLoading)
        {
            if (isLoading)
            {
                LstOrders.Visibility = Visibility.Hidden;
                TxtLoading.Visibility = Visibility.Visible;
            }
            else
            {
                TxtLoading.Visibility = Visibility.Hidden;
                LstOrders.Visibility = Visibility.Visible;
            }
        }
    }
}
