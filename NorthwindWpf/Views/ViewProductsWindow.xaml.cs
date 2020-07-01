using AutoMapper;
using Northwind.Data;
using NorthwindWpf.Core.Service;
using NorthwindWpf.Data.Repositories;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using WpfApp1.Models;
using WpfApp1.ViewModels;

namespace WpfApp1.Views
{
    public partial class ViewProductsWindow : Window
    {
        private readonly IProductRepository _productRepo;
        private readonly ViewProductsViewModel _viewModel;

        private IEnumerable<Product> _allProducts;

        public ViewProductsWindow()
        {
            InitializeComponent();
            Loaded += OnWindowLoaded;

            _productRepo = ServiceResolver.Get().Resolve<IProductRepository>();

            _viewModel = new ViewProductsViewModel();
            DataContext = _viewModel;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            _productRepo.Dispose();
        }

        private async void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            _allProducts = await _productRepo.GetAll().OrderBy(p => p.ProductName).ToArrayAsync();

            LoadProductsIntoGrid();

            TxtLoading.Visibility = Visibility.Hidden;
            StkProducts.Visibility = Visibility.Visible;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            LoadProductsIntoGrid();
        }

        private void OnProductSelected(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //TODO open new window
        }

        private void LoadProductsIntoGrid()
        {
            var products = _allProducts;

            if (!_viewModel.ShowDiscontinued)
            {
                products = products.Where(p => !p.Discontinued);
            }

            _viewModel.Products = Mapper.Map<IEnumerable<ProductLineModel>>(products);
        }
    }
}
