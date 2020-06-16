using Northwind.Data;
using NorthwindWpf.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.ViewModels;

namespace WpfApp1.Views
{
    public partial class OrderEntryWindow : Window, INotifyPropertyChanged
    {
        private OrderViewModel _viewModel;
        private bool _windowReady;

        public event PropertyChangedEventHandler PropertyChanged;

        public int? OrderId { get; set; }

        public bool WindowReady 
        { 
            get => _windowReady; 
            set 
            {
                _windowReady = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindowReady)));
            }
        }

        public OrderEntryWindow()
        {
            InitializeComponent();
            WindowReady = false;
        }

        #region Event Handlers

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (var customerRepo = new CustomerRepository())
            using (var shipperRepo = new ShipperRepository())
            {
                _viewModel = new OrderViewModel
                {
                    Customers = await customerRepo.GetAll().OrderBy(x => x.CompanyName).ToArrayAsync(),
                    Shippers = await shipperRepo.GetAll().OrderBy(x => x.CompanyName).ToArrayAsync()
                };

                if (OrderId.HasValue)
                {
                    using(var orderRepo = new OrderRepository())
                    {
                        var order = await orderRepo.GetByIdAsync(OrderId.Value);
                        _viewModel.Customer = order.Customer;
                        _viewModel.Shipper = order.Shipper;
                        _viewModel.OrderDate = order.OrderDate;
                        _viewModel.RequiredDate = order.RequiredDate;
                        _viewModel.LineItems = new ObservableCollection<OrderViewModel.LineItem>(order.Order_Details.Select(x => new OrderViewModel.LineItem
                        {
                            Product = x.Product,
                            Qty = x.Quantity,
                            Discount = x.Discount * 100
                        }));

                        CmbCustomer.SelectedItem = order.Customer;
                        CmbShipMethod.SelectedItem = order.Shipper;

                    }
                }
                else//new order
                {
                    _viewModel.LineItems = new ObservableCollection<OrderViewModel.LineItem>();
                }
            }

            _viewModel.LineItems.CollectionChanged += OnLineItemsChanged;
            DataContext = _viewModel;

            WindowReady = true;
        }

        private void OnLineItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CalculateOrderTotal();
        }

        private void AddLineItem_Click(object sender, RoutedEventArgs e)
        {
            var addItemWindow = new AddItemWindow
            {
                ExcludeProducts = _viewModel.LineItems.Select(x => x.Product)
            };

            if (addItemWindow.ShowDialog() == true)
            {
                var item = new OrderViewModel.LineItem
                {
                    Product = addItemWindow.Product,
                    Qty = addItemWindow.Quantity,
                };

                item.PropertyChanged += (s, args) =>
                {
                    CalculateOrderTotal();
                };

                _viewModel.LineItems.Add(item);
            }
        }

        private void OnDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DateOrder.SelectedDate == null || DateRequired.SelectedDate == null) return;

            if (DateRequired.SelectedDate < DateOrder.SelectedDate)
            {
                MessageBox.Show("Required date cannot be before order date");
                DateRequired.SelectedDate = DateOrder.SelectedDate;
            }
        }

        private void OnOrderSave(object sender, RoutedEventArgs e)
        {
            var errors = new List<string>();

            if (_viewModel.Customer == null) errors.Add("Please select a customer");

            if (_viewModel.Shipper == null) errors.Add("Please select a shipping method");
        
            if (_viewModel.OrderDate == null) errors.Add("An order date is required");
        
            if (!_viewModel.LineItems.Any()) errors.Add("Please add at least 1 line item");

            if (errors.Any())
            {
                var message = new StringBuilder("Please address the following errors:").Append(Environment.NewLine);

                foreach(var error in errors)
                {
                    message
                        .Append(Environment.NewLine)
                        .Append(error);
                }

                MessageBox.Show(message.ToString());
            }
            else
            {
                //TODO commit order to database
                DialogResult = true;
                Close();
            }
        }

        #endregion

        private void CalculateOrderTotal()
        {
            _viewModel.OrderTotal = _viewModel.LineItems.Sum(x => x.TotalPrice);
            TxtTotal.Text = _viewModel.OrderTotal.ToString("0.00");
        }
    }
}