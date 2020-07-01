using Northwind.Data;
using System.Collections.Generic;
using WpfApp1.Models;

namespace WpfApp1.ViewModels
{
    public class ViewProductsViewModel : ViewModelBase
    {
        private IEnumerable<ProductLineModel> _products;
        private bool _showDiscontinued;

        public bool ShowDiscontinued { get => _showDiscontinued; set => SetProperty(ref _showDiscontinued, value); }

        public IEnumerable<ProductLineModel> Products { get => _products; set => SetProperty(ref _products, value); }
    }
}
