using NorthwindWpf;
using NorthwindWpf.Data.Models;
using NorthwindWpf.Data.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Views
{
    public partial class AddressSearchWindow : Window
    {
        private readonly IAddressLookupService _addressService;

        public AddressFindResult.Address SelectedAddress { get; private set; }

        public AddressSearchWindow()
        {
            InitializeComponent();

            var app = Application.Current as App;
            _addressService = app.GetService<IAddressLookupService>();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _addressService.Dispose();
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            var postCode = TxtPostCode.Text;
            var number = 0;

            if (string.IsNullOrWhiteSpace(postCode)) return; //TODO need an error message

            if (!string.IsNullOrWhiteSpace(TxtNumber.Text))
            {
                if (!int.TryParse(TxtNumber.Text.Trim(), out number))
                {
                    //TODO need an error message
                    return;
                }
            }

            var addressResults = await _addressService.FindByPostCodeAsync(postCode, number == default ? (int?)null: number);
            DgResults.ItemsSource = addressResults.Addresses;
        }

        private void Result_DoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var row = (DataGridRow) sender;
            SelectedAddress = row.Item as AddressFindResult.Address;

            if (SelectedAddress != null)
            {
                SelectedAddress.PostCode = TxtPostCode.Text.ToUpperInvariant();
                DialogResult = true;
                Close();
            }
        }
    }
}
