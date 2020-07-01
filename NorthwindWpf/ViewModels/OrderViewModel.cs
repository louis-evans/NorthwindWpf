using Northwind.Data;
using NorthwindWpf.Core.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WpfApp1.ViewModels
{
    public class OrderViewModel : ViewModelBase
    {
        private Customer _customer;
        private string _shipCity;
        private string _shipAddress;
        private Shipper _shipper;
        private string shipRegion;
        private string shipPostCode;
        private string shipCountry;
        private decimal _orderTotal;

        public int? OrderID { get; set; }
        public Customer Customer { get => _customer; set => SetProperty(ref _customer, value); }
        public Shipper Shipper { get => _shipper; set => SetProperty(ref _shipper, value); }
        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get => _shipAddress; set => SetProperty(ref _shipAddress, value); }
        public string ShipCity { get => _shipCity; set => SetProperty(ref _shipCity, value); }
        public string ShipRegion { get => shipRegion; set => SetProperty(ref shipRegion, value); }
        public string ShipPostCode { get => shipPostCode; set => SetProperty(ref shipPostCode, value); }
        public string ShipCountry { get => shipCountry; set => SetProperty(ref shipCountry, value); }
        public decimal OrderTotal { get => _orderTotal; set => SetProperty(ref _orderTotal, value); }

        public ObservableCollection<LineItem> LineItems { get; set; }

        public IEnumerable<Customer> Customers { get; set; }
        public IEnumerable<Shipper> Shippers { get; set; }

        public class LineItem : ViewModelBase
        {
            private int _qty;
            private float _discount;
            private decimal _total;
            private string _totalDisplay;
            private decimal _unitPrice;

            public Product Product { get; set; }

            public string ProductDisplay => Product.ProductName;

            public decimal UnitPrice
            {
                get => _unitPrice;
                set
                {
                    SetProperty(ref _unitPrice, decimal.Round(Math.Max(0m, value), 2, MidpointRounding.AwayFromZero));
                    CalculateTotal();
                }
            }

            public int Qty
            {
                get => _qty;
                set
                {
                    SetProperty(ref _qty, Math.Max(0, value));
                    CalculateTotal();
                }
            }

            public float Discount
            {
                get => _discount;
                set
                {
                    SetProperty(ref _discount, Math.Max(0f, Math.Min(100f, value)));
                    CalculateTotal();
                }
            }

            public decimal TotalPrice
            {
                get => _total;
                private set
                {
                    SetProperty(ref _total, Math.Max(0m, value));
                    TotalPriceDisplay = _total.ToString("0.00");
                }
            }

            public string TotalPriceDisplay
            {
                get => _totalDisplay;
                private set => SetProperty(ref _totalDisplay, value);
            }

            private void CalculateTotal()
            {
                TotalPrice = OrderUtils.CalculateLineTotal(UnitPrice, Qty, Discount);
            }
        }
    }
}
