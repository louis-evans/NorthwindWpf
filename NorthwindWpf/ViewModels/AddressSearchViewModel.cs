using NorthwindWpf.Data.Models;
using System.Collections.Generic;

namespace WpfApp1.ViewModels
{
    public class AddressSearchViewModel : ViewModelBase
    {
        private string _postCode;
        private string _nameNumber;
        private IEnumerable<AddressFindResult.Address> _addresses;

        public string PostCode { get => _postCode; set => SetProperty(ref _postCode, value); }
        public string NameNumber { get => _nameNumber; set => SetProperty(ref _nameNumber, value); }
        public IEnumerable<AddressFindResult.Address> Addresses { get => _addresses; set => SetProperty(ref _addresses, value); }
    }
}
