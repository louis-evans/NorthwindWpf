using NorthwindWpf.Core.Service;
using NorthwindWpf.Core.Services;
using NorthwindWpf.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using WpfApp1.ViewModels;

namespace WpfApp1.Views
{
    public partial class AddressSearchWindow : Window
    {
        private readonly IAddressLookupService _addressService;

        public AddressFindResult.Address SelectedAddress { get; private set; }
        private readonly AddressSearchViewModel _viewModel;

        public AddressSearchWindow()
        {
            InitializeComponent();

            _addressService = ServiceResolver.Get().Resolve<IAddressLookupService>();

            _viewModel = new AddressSearchViewModel
            {
                Addresses = new List<AddressFindResult.Address>()
            };

            DataContext = _viewModel;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _addressService.Dispose();
        }

        //TODO 
        // Implement a ICommand in the view model to handle the search event for both button and enter key

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            RunSearch();
        }

        private void OnInputKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RunSearch();
            }
            else
            {
                var txtSender = (TextBox)sender;

                //do this because the view model value doent get updated until the field loses focus
                if (txtSender == TxtPostCode) _viewModel.PostCode = txtSender.Text;
                else if (txtSender == TxtNameNumber) _viewModel.NameNumber = txtSender.Text;
            }
        }

        private void Result_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            var row = (DataGridRow) sender;
            SelectedAddress = row.Item as AddressFindResult.Address;

            if (SelectedAddress != null)
            {
                SelectedAddress.PostCode = _viewModel.PostCode.ToUpperInvariant();
                DialogResult = true;
                Close();
            }
        }

        private async void RunSearch()
        {
            var number = 0;

            if (string.IsNullOrWhiteSpace(_viewModel.PostCode)) return; //TODO need an error message

            if (!string.IsNullOrWhiteSpace(_viewModel.NameNumber))
            {
                if (!int.TryParse(_viewModel.NameNumber.Trim(), out number))
                {
                    //TODO need an error message
                    return;
                }
            }

            var addressResults = await _addressService.FindByPostCodeAsync(_viewModel.PostCode, number == default ? (int?)null : number);
            _viewModel.Addresses = new List<AddressFindResult.Address>(addressResults.Addresses);
        }
    }
}
