using NorthwindWpf.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using Northwind.Data;

namespace WpfApp1.Views
{
    public partial class AddItemWindow : Window
    {
        public Product Product { get; private set; }
        public int Quantity { get; private set; }
        public IEnumerable<Product> ExcludeProducts { get; set; }

        public AddItemWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (var productRepo = new ProductRepository())
            {
                IQueryable<Product> productQuery = productRepo.GetAll();

                if (ExcludeProducts != null)
                {
                    var excludedProductIds = ExcludeProducts.Select(x => x.ProductID).ToList();
                    productQuery = productQuery.Where(p => !excludedProductIds.Contains(p.ProductID));
                }

                CmbCustomers.ItemsSource = await productQuery.OrderBy(p => p.ProductName).ToArrayAsync();
                CmbCustomers.DisplayMemberPath = "ProductName";
                CmbCustomers.SelectedValuePath = "ProductID";

                TxtQty.Text = "0";
            }
        }

        private void OnSubmit(object sender, RoutedEventArgs e)
        {
            if (Product != null && Quantity > 0)
            {
                DialogResult = true;
                Close();
            }
        }

        private void CmbCustomers_Selected(object sender, RoutedEventArgs e)
        {
            Product = CmbCustomers.SelectedItem as Product;
        }

        private void TxtQty_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            int qty;
            int.TryParse(TxtQty.Text, out qty);
            Quantity = qty;
            
        }
    }
}
