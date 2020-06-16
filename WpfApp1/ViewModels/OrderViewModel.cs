using Northwind.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WpfApp1.ViewModels
{
    public class OrderViewModel
    {
        public int? OrderID { get; set; }
        public Customer Customer { get; set; }
        public Shipper Shipper { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public decimal OrderTotal { get; set; }
        public ObservableCollection<LineItem> LineItems { get; set; }

        public IEnumerable<Customer> Customers { get; set; }
        public IEnumerable<Shipper> Shippers { get; set; }

        public class LineItem : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            private int _qty;
            private float _discount;
            private decimal _total;
            private string _totalDisplay;

            public Product Product { get; set; }

            public int Qty 
            { 
                get
                {
                    return _qty;                        
                }
                set
                {
                    _qty = Math.Max(0, value);
                    OnPropertyChanged(nameof(Qty));
                    CalculateTotal();
                }
            }

            public float Discount
            {
                get
                {
                    return _discount;
                }
                set
                {
                    _discount = Math.Max(0f, Math.Min(100f, value));
                    OnPropertyChanged(nameof(Discount));
                    CalculateTotal();
                }
            }

            public decimal TotalPrice
            {
                get
                {
                    return _total;
                }
                private set
                {
                    _total = Math.Max(0m, value);
                    TotalPriceDisplay = _total.ToString("0.00");
                    OnPropertyChanged(nameof(TotalPrice));
                }
            }

            public string ProductDisplay => Product.ProductName;

            public decimal UnitPrice => Product.UnitPrice ?? 0m;

            public string UnitPriceDisplay => Product.UnitPrice?.ToString("0.00") ?? "No price available";

            public string TotalPriceDisplay 
            {
                get
                {
                    return _totalDisplay;
                }
                private set
                {
                    _totalDisplay = value;
                    OnPropertyChanged(nameof(TotalPriceDisplay));
                }
            }

            protected virtual void OnPropertyChanged(string property)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
            }

            private void CalculateTotal()
            {
                var netTotal = UnitPrice * Qty;
                var discountTotal = netTotal * ((decimal) Discount / 100);
                TotalPrice = netTotal - discountTotal;
            }
        }
    }
}
