using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using OrderingSystemService; // Import the service namespace
using OrderingSystemDTO; // Import the DTO namespace

namespace OrderingSystem.ViewModels
{
    public class ChangeStatusViewModel : BaseViewModel, IQueryAttributable
    {
        private string _selectedStatus;
        private int _orderId;
        private readonly IOrderService _orderService; // Inject the order service

        public string SelectedStatus
        {
            get { return _selectedStatus; }
            set { SetProperty(ref _selectedStatus, value); }
        }

        public IList<string> StatusOptions { get; private set; }

        public ICommand ConfirmCommand { get; private set; }

        public event EventHandler<string> StatusChanged;

        public IDictionary<string, object> Query { get; set; }

        public ChangeStatusViewModel(IOrderService orderService)
        {
            _orderService = orderService; // Initialize the order service

            // Populate status options
            StatusOptions = new List<string> { "Hoàn tất", "Chưa thanh toán", "Mới tạo" };

            // Command to confirm status change
            ConfirmCommand = new Command(async () => await Confirm());
        }

        private async Task Confirm()
        {
            try
            {
                // Update the status using the service
                var order = await _orderService.GetOrderById(_orderId);
                order.Status = SelectedStatus;
                await _orderService.UpdateOrder(order.OrderID, order);

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("OrderId"))
            {
                if (int.TryParse(query["OrderId"].ToString(), out int orderId))
                {
                    _orderId = orderId;
                }
            }
        }
    }
}
