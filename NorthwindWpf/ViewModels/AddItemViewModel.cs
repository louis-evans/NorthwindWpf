using Northwind.Data;
using System.Collections.Generic;

namespace WpfApp1.ViewModels
{
    public class AddItemViewModel : ViewModelBase
    {
        private IEnumerable<Product> _products;
        private int _qty;

        public int Quantity { get => _qty; set => SetProperty(ref _qty, value); }
        public IEnumerable<Product> Products { get => _products; set => SetProperty(ref _products, value); }
    }
}
