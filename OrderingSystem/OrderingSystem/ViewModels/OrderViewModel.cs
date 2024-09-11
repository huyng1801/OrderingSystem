using OrderingSystem.Views.Admin.Order;
using OrderingSystemDTO;
using OrderingSystemService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OrderingSystem.ViewModels
{
    public class OrderViewModel : BaseViewModel
    {
       
        private readonly IOrderService _orderService;

        private ObservableCollection<OrderDTO> _orders;
        private OrderDTO _selectedOrder;
        public ICommand ViewOrderDetailsCommand { get; private set; }

        public ICommand ChangeStatusCommand { get; private set; }
        public OrderViewModel(IOrderService orderService)
        {
            _orderService = orderService;
        
            Orders = new ObservableCollection<OrderDTO>();

            ViewOrderDetailsCommand = new Command<OrderDTO>(async (order) => await ViewOrderDetails(order));

            ChangeStatusCommand = new Command<OrderDTO>(async (order) => await UpdateStatus(order));
            LoadOrders();
        }

        public OrderDTO SelectedOrder
        {
            get { return _selectedOrder; }
            set { SetProperty(ref _selectedOrder, value); }
        }

        private async Task UpdateStatus(OrderDTO order)
        {
            try
            {
                var navigationParameter = new Dictionary<string, object>
                    {
                        { "OrderId", order.OrderID }
                    };
                await Shell.Current.GoToAsync($"{nameof(ChangeStatusPage)}", navigationParameter);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Lỗi: {ex.Message}");
            }
        }
        public ObservableCollection<OrderDTO> Orders
        {
            get { return _orders; }
            set
            {
                SetProperty(ref _orders, value);
            }
        }
   
        public async void LoadOrders()
        {
            try
            {
                var orders = await _orderService.GetAllOrders();
                Orders = new ObservableCollection<OrderDTO>(orders);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
     
        private async Task ViewOrderDetails(OrderDTO order)
        {
            try
            {
                var navigationParameter = new Dictionary<string, object>
                    {
                        { "OrderId", order.OrderID }
                    };
                await Shell.Current.GoToAsync($"{nameof(OrderDetailPage)}", navigationParameter);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Lỗi: {ex.Message}");
            }
        }
    }
 

}
