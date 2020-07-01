using System.Collections.Generic;
using WpfApp1.Models;

namespace WpfApp1.ViewModels
{
    public class ViewOrdersViewModel : ViewModelBase
    {
        private IEnumerable<OrderLineModel> _orderLines;

        public IEnumerable<OrderLineModel> OrderLines { get => _orderLines; set => SetProperty(ref _orderLines, value); }
    }
}
