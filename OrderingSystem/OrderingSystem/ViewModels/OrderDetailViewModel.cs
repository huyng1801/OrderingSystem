using OrderingSystemDTO;
using OrderingSystemService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OrderingSystem.ViewModels
{
    public class OrderDetailViewModel : BaseViewModel, IQueryAttributable
    {
        private int _orderId;
        private ObservableCollection<OrderDetailDTO> _orderDetails;
        private readonly IOrderDetailService _orderDetailService;
        public ICommand UpdateIsServedCommand { get; }

        public OrderDetailViewModel(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
            OrderDetails = new ObservableCollection<OrderDetailDTO>();
            UpdateIsServedCommand = new Command<OrderDetailDTO>(async (orderDetail) => await UpdateIsServed(orderDetail));
        }

        public ObservableCollection<OrderDetailDTO> OrderDetails
        {
            get { return _orderDetails; }
            set { SetProperty(ref _orderDetails, value); }
        }

        public async Task Initialize(int orderId)
        {
            _orderId = orderId;
            try
            {
                var orderDetails = await _orderDetailService.GetOrderDetailsByOrderId(_orderId);
                if (orderDetails != null)
                {
                    OrderDetails = new ObservableCollection<OrderDetailDTO>(orderDetails);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private async Task UpdateIsServed(OrderDetailDTO orderDetail)
        {
            orderDetail.IsServed = true;

            await _orderDetailService.UpdateOrderDetail(orderDetail.OrderDetailID, orderDetail);
            Initialize(_orderId);
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("OrderId"))
            {
                if (int.TryParse(query["OrderId"].ToString(), out int orderId))
                {
                    _orderId = orderId;
                    Initialize(orderId);
                }
            }
        }
    }
}
