using System.Linq;
using System.Windows;
using NorthwindWpf.ViewModels;
using NorthwindWpf.Data.Repositories;
using WpfApp1.Views;
using System.Threading.Tasks;
using NorthwindWpf.Core.Utils;
using NorthwindWpf.Core.Service;
using AutoMapper;
using System.Collections.Generic;

namespace NorthwindWpf.Views
{
    public partial class ViewOrderWindow : Window
    {
        private const string DATE_FORMAT = "dd/MM/yyyy";
        private readonly IOrderRepository _orderRepo;

        public int OrderId { get; private set; }

        public ViewOrderWindow(int orderId)
        {
            InitializeComponent();
            OrderId = orderId;

            _orderRepo = ServiceResolver.Get().Resolve<IOrderRepository>();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadOrder();
        }

        private async void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var window = new OrderEntryWindow
            {
                OrderId = OrderId
            };

            if (window.ShowDialog() == true)
            {
                await LoadOrder();
            }
        }

        private async Task LoadOrder()
        {
            var order = await _orderRepo.GetByIdAsync(OrderId);

            var viewModel = Mapper.Map<ViewOrderViewModel>(order);
            viewModel.LineItems = Mapper.Map<IEnumerable<ViewOrderViewModel.LineItemModel>>(order.Order_Details);

            DataContext = viewModel;
        }
    }
}