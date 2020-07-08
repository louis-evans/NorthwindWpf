using NorthwindWpf.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using Northwind.Data;
using NorthwindWpf.Core.Service;
using WpfApp1.ViewModels;

namespace WpfApp1.Views
{
    public partial class AddItemWindow : Window
    {
        private readonly IProductRepository _productRepo;
        private readonly AddItemViewModel _viewModel;

        public IEnumerable<Product> ExcludeProducts { private get; set; }

        public AddItemResult Result { get; private set; }

        public AddItemWindow()
        {
            InitializeComponent();

            _productRepo = ServiceResolver.Get().Resolve<IProductRepository>();

            _viewModel = new AddItemViewModel();
            DataContext = _viewModel;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _productRepo.Dispose();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var productQuery = _productRepo.GetAll();

            if (ExcludeProducts != null)
            {
                var excludedProductIds = ExcludeProducts.Select(x => x.ProductID).ToList();
                productQuery = productQuery.Where(p => !excludedProductIds.Contains(p.ProductID));
            }

            _viewModel.Products = await productQuery.OrderBy(p => p.ProductName).ToArrayAsync();
        }

        private void OnSubmit(object sender, RoutedEventArgs e)
        {
            if (CmbProducts.SelectedItem != null && _viewModel.Quantity > 0)
            {
                int qty;

                if (int.TryParse(TxtQty.Text, out qty))
                {
                    Result = new AddItemResult
                    {
                        Product = CmbProducts.SelectedItem as Product,
                        Quantity = qty
                    };

                    DialogResult = true;
                    Close();
                }
            }
        }

        public class AddItemResult
        {
            public Product Product { get; set; }
            public int Quantity { get; set; }
        }
    }
}
