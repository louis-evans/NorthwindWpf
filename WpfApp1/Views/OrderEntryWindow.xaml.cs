using AutoMapper;
using Northwind.Data;
using NorthwindWpf;
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
        private readonly ICustomerRepository _customerRepo;
        private readonly IShipperRepository _shipperRepo;
        private readonly IOrderRepository _orderRepo;

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

            var app = (App)Application.Current;
            _customerRepo = app.GetService<ICustomerRepository>();
            _shipperRepo = app.GetService<IShipperRepository>();
            _orderRepo = app.GetService<IOrderRepository>();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _customerRepo.Dispose();
            _shipperRepo.Dispose();
            _orderRepo.Dispose();
        }

        #region Event Handlers

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel = new OrderViewModel();

            if (OrderId.HasValue)
            {
                var order = await _orderRepo.GetByIdAsync(OrderId.Value);

                _viewModel = Mapper.Map<OrderViewModel>(order);
    
                var lineItems = Mapper.Map<IEnumerable<OrderViewModel.LineItem>>(order.Order_Details);

                foreach(var line in lineItems)
                {
                    line.PropertyChanged += OnLineItemPropertyChanged;
                }
                
                _viewModel.LineItems = new ObservableCollection<OrderViewModel.LineItem>(lineItems);

                CmbCustomer.SelectedItem = order.Customer;
                CmbShipMethod.SelectedItem = order.Shipper;

                CalculateOrderTotal();
            }
            else//new order
            {
                _viewModel.LineItems = new ObservableCollection<OrderViewModel.LineItem>();
            }

            _viewModel.LineItems.CollectionChanged += OnLineItemsChanged;

            _viewModel.Customers = await _customerRepo.GetAll().OrderBy(x => x.CompanyName).ToArrayAsync();
            _viewModel.Shippers = await _shipperRepo.GetAll().OrderBy(x => x.CompanyName).ToArrayAsync();

            DataContext = _viewModel;

            WindowReady = true;
        }

        private void OnLineItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CalculateOrderTotal();
        }

        private void OnLineItemPropertyChanged(object sender, PropertyChangedEventArgs arg)
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
                    UnitPrice = addItemWindow.Product.UnitPrice ?? 0m,
                    Qty = addItemWindow.Quantity,
                };

                item.PropertyChanged += OnLineItemPropertyChanged;

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

        private async void OnOrderSave(object sender, RoutedEventArgs e)
        {
            var errors = GetValidationErrors();

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
                Order order;

                if (OrderId.HasValue)
                {
                    order = await _orderRepo.GetByIdAsync(OrderId.Value);

                    var deletedItems = order.Order_Details.Where(x => !_viewModel.LineItems.Select(l => l.Product.ProductID).Contains(x.ProductID));

                    foreach(var deletedItem in deletedItems)
                    {
                        order.Order_Details.Remove(deletedItem);
                    }

                    foreach (var newItem in _viewModel.LineItems)
                    {
                        Order_Detail lineItem;

                        if (order.Order_Details.Any(x => x.ProductID == newItem.Product.ProductID))
                        {
                            lineItem = order.Order_Details.First(x => x.ProductID == newItem.Product.ProductID);
                        }
                        else
                        {
                            lineItem = new Order_Detail
                            {
                                Product = newItem.Product,
                            };

                            order.Order_Details.Add(lineItem);
                        }

                        lineItem.UnitPrice = newItem.UnitPrice;
                        lineItem.Quantity = Convert.ToInt16(newItem.Qty);
                        lineItem.Discount = newItem.Discount / 100;
                    }
                }
                else
                {
                    order = new Order();

                    foreach (var newItem in _viewModel.LineItems)
                    {
                        order.Order_Details.Add(new Order_Detail
                        {
                            ProductID = newItem.Product.ProductID,
                            UnitPrice = newItem.Product.UnitPrice ?? 0m,
                            Quantity = Convert.ToInt16(newItem.Qty),
                            Discount = newItem.Discount / 100
                        });
                    }
                }

                order.CustomerID = _viewModel.Customer.CustomerID;
                order.ShipVia = _viewModel.Shipper.ShipperID;
                order.OrderDate = _viewModel.OrderDate;
                order.RequiredDate = _viewModel.RequiredDate;

                _orderRepo.SaveOrder(order);
                    
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

        private IEnumerable<string> GetValidationErrors()
        {
            var errors = new List<string>();

            if (_viewModel.Customer == null) errors.Add("Please select a customer");

            if (_viewModel.Shipper == null) errors.Add("Please select a shipping method");

            if (_viewModel.OrderDate == null) errors.Add("An order date is required");

            if (!_viewModel.LineItems.Any()) errors.Add("Please add at least 1 line item");

            return errors;
        }

        private void PostCodeSearchClick(object sender, RoutedEventArgs e)
        {
            var addressWindow = new AddressSearchWindow();

            if (addressWindow.ShowDialog() == true && addressWindow.SelectedAddress != null)
            {
                _viewModel.ShipAddress = addressWindow.SelectedAddress.Line1;
                _viewModel.ShipPostCode = addressWindow.SelectedAddress.PostCode;
                _viewModel.ShipCity = addressWindow.SelectedAddress.TownCity.Replace(" ", "");
                _viewModel.ShipRegion = addressWindow.SelectedAddress.County;
                _viewModel.ShipCountry = addressWindow.SelectedAddress.Country;
            }
        }
    }
}