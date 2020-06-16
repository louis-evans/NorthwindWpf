﻿using Northwind.Data;
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
        private readonly NorthwindEntities _ctx;
        private OrderViewModel _viewModel;
        private bool _windowReady;

        public event PropertyChangedEventHandler PropertyChanged;

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
            _ctx = new NorthwindEntities();
            WindowReady = false;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _ctx.Dispose();
        }

        #region Event Handlers

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel = new OrderViewModel
            {
                LineItems = new ObservableCollection<OrderViewModel.LineItem>(),
                Customers = await _ctx.Customers.OrderBy(x => x.CompanyName).ToArrayAsync(),
                Shippers = await _ctx.Shippers.OrderBy(x => x.CompanyName).ToArrayAsync()
            };

            if (/* Existing order */false)
            {
                //TODO: Load order and set view model values
            }
            else//new order
            {
                //initialise any empty values
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
        }

        #endregion

        private void CalculateOrderTotal()
        {
            _viewModel.OrderTotal = _viewModel.LineItems.Sum(x => x.TotalPrice);
            TxtTotal.Text = _viewModel.OrderTotal.ToString("0.00");
        }
    }
}