using NorthwindWpf.Data.Models;
using NorthwindWpf.Data.Services;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Views
{
    /// <summary>
    /// Interaction logic for AddressSearchWindow.xaml
    /// </summary>
    public partial class AddressSearchWindow : Window
    {
        public AddressFindResult.Address SelectedAddress { get; private set; }

        public AddressSearchWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

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

            using (var addressService = new AddressLookupService())
            {
                var addressResults = await addressService.FindByPostCodeAsync(postCode, number == default ? (int?)null: number);
                DgResults.ItemsSource = addressResults.Addresses;
            }
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
