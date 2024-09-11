using OrderingSystemCustomerService;
using OrderingSystemCustomerDTO;
using OrderingSystemCustomer.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Text;


namespace OrderingSystemCustomer.ViewModels
{
    public class OrderViewModel : BaseViewModel
    {
        private readonly OrderDetailService _orderDetailservice;
        private readonly CategoryService _categoryService;
        private readonly ProductService _productService;
        private readonly OrderService _orderService;
        private readonly TableService _tableService;
        private bool _isOverlay;
        private string _currentTableID;
        private bool _dynamicGrid;
        private long _totalAmount;

        private ObservableCollection<CategoryDTO> _categories;
        private ObservableCollection<ProductDTO> _products;
        private ObservableCollection<OrderDetailDTO> _orderDetails;
        public ICommand LoadOrderDetailsFromServerCommand { get; }
        public ICommand ShowCalledItemsCommand { get; }
        public ICommand ShowOrderedItemsCommand { get; }
        private ICommand _loadProductsCommand;
        public ICommand ShowDetailsCommand { get; }
        public ICommand IncreaseQuantityCommand { get; }
        public ICommand DecreaseQuantityCommand { get; }
        public ICommand SendOrderCommand { get; }
        public ICommand TapGestureRecognizer { get; private set; }
        public ICommand ExitCommand { get; private set; }
        public ICommand AddToOrderCommand { get; }
        public ICommand TapOutsideCommand { get; private set; }
        public ICommand PaymentCommand { get; }
        public OrderViewModel()
        {
            _categoryService = new CategoryService();
            _productService = new ProductService();
            _orderDetailservice = new OrderDetailService();
            _orderService = new OrderService();
            _tableService = new TableService();
            OrderDetails = new ObservableCollection<OrderDetailDTO>();
            TapGestureRecognizer = new Command(OnTapOutside);
            IncreaseQuantityCommand = new Command<OrderDetailDTO>(IncreaseQuantityInOrderDetails);
            DecreaseQuantityCommand = new Command<OrderDetailDTO>(DecreaseQuantityInOrderDetails);
            ExitCommand = new Command(Exit);
            SendOrderCommand = new Command(async () => await SendOrderDetail());
            AddToOrderCommand = new Command<ProductDTO>(async (product) => await AddToOrderAsync(product));
            TapOutsideCommand = new Command(OnTapOutside);
            ShowDetailsCommand = new Command(ShowDetails);
            LoadOrderDetailsFromServerCommand = new Command(async () => await LoadOrderDetailsFromServerAsync());
            ShowCalledItemsCommand = new Command(async () => await ShowCalledItems());
            ShowOrderedItemsCommand = new Command(async () => await LoadOrderedItemsAsync());
            PaymentCommand = new Command(async () => await ExecutePayment());
            LoadCategoriesAndProducts();

        }
        private async Task ExecutePayment()
        {
            try
            {
                bool proceedPayment = await App.Current.MainPage.DisplayAlert("Xác nhận thanh toán", "Bạn có chắc chắn muốn thanh toán đơn hàng?", "Đồng ý", "Hủy");

                if (proceedPayment)
                {
                    await ShowOrderItemsDialog();
                    Session.OrderDetails.Clear();
                  
                    OrderDetails.Clear();
                    TotalAmount = 0;

                    var order = await _orderService.GetOrderById(Session.CurrentOrderID);
                    
                    if (order != null)
                    {
                        order.Status = "Chưa thanh toán";
                        await _orderService.UpdateOrder(order.OrderID, order);
                       
                      
                    }

                    Session.CurrentOrderID = 0;
                    
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Thanh toán thành công. Đơn hàng đã được đặt lại.", "OK");
                    await NavigationService.Navigation.PopModalAsync();
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Lỗi", $"Đã xảy ra lỗi trong quá trình thanh toán: {ex.Message}", "OK");
            }
        }

        private async Task ShowOrderItemsDialog()
        {
            var orderDetails = await _orderDetailservice.GetOrderDetailsByOrderId(Session.CurrentOrderID);
            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder.AppendLine("Danh sách các món trong đơn hàng:");

            decimal totalAmount = 0;

            foreach (var orderDetail in orderDetails)
            {
                decimal totalPrice = orderDetail.Quantity * orderDetail.UnitePrice;
                totalAmount += totalPrice;

                messageBuilder.AppendLine($"{orderDetail.ProductName} - Số lượng: {orderDetail.Quantity} - Đơn giá: {orderDetail.UnitePrice} - Tổng tiền: {totalPrice}");
            }

            messageBuilder.AppendLine($"Tổng tiền đơn hàng: {totalAmount}");

            await App.Current.MainPage.DisplayAlert("Danh sách đơn hàng", messageBuilder.ToString(), "OK");
        }




        public bool DynamicGrid
        {
            get { return _dynamicGrid; }
            set
            {
                if (_dynamicGrid != value)
                {
                    _dynamicGrid = value;
                    OnPropertyChanged();
                }
            }
        }
        public long TotalAmount
        {
            get { return _totalAmount; }
            set
            {
                if (_totalAmount != value)
                {
                    _totalAmount = value;
                    OnPropertyChanged();
                }
            }
        }
        public string CurrentTableID
        {
            get { return _currentTableID; }
            set
            {
                SetProperty(ref _currentTableID, value);
            }
        }
        public bool IsOverlay
        {
            get { return _isOverlay; }
            set { SetProperty(ref _isOverlay, value); }
        }

        public ObservableCollection<OrderDetailDTO> OrderDetails
        {
            get { return _orderDetails; }
            set { SetProperty(ref _orderDetails, value); }
        }

        public ObservableCollection<CategoryDTO> Categories
        {
            get { return _categories; }
            set { SetProperty(ref _categories, value); }
        }

        public ObservableCollection<ProductDTO> Products
        {
            get { return _products; }
            set { SetProperty(ref _products, value); }
        }
        public ICommand LoadProductsCommand
        {
            get
            {
                return _loadProductsCommand ?? (_loadProductsCommand = new Command<int>(async (categoryId) => await LoadProductsByCategory(categoryId)));
            }
        }
        private async Task ShowCalledItems()
        {
            OrderDetails.Clear();
            UpdateOrderDetails();
        
        }

        private void UpdateOrderDetails()
        {
            
            DynamicGrid = false;
            foreach (var sessionOrderDetail in Session.OrderDetails)
            {
                var existingOrderDetail = OrderDetails.FirstOrDefault(od => od.ProductID == sessionOrderDetail.ProductID && od.OrderID == sessionOrderDetail.OrderID);

                if (existingOrderDetail == null)
                {
                    OrderDetails.Add(sessionOrderDetail);
                }
                else
                {
                    existingOrderDetail.Quantity = sessionOrderDetail.Quantity;

                    if (!Session.OrderDetails.Contains(existingOrderDetail))
                    {
                        OrderDetails.Remove(existingOrderDetail);
                    }
                }
            }

            foreach (var orderDetail in OrderDetails.ToList())
            {

                orderDetail.IsOrdered = false;
                
                if (!Session.OrderDetails.Contains(orderDetail))
                {
                    OrderDetails.Remove(orderDetail);
                }
            }
           
        }



        private async Task LoadCategories()
        {
            var categories = await _categoryService.GetAllCategories();
            foreach (var category in categories)
            {
                category.Color = CustomColor.White;
            }
            Categories = new ObservableCollection<CategoryDTO>(categories);
        }
        private async void LoadCategoriesAndProducts()
        {
            await LoadCategories();
            if (Categories.Any())
            {
                await LoadProductsByCategory(Categories.First().CategoryID);
            }
            CurrentTableID = "Bàn số " + Session.CurrentTableID;
        }

        private async Task LoadProductsByCategory(int categoryId)
        {
            foreach (var category in Categories)
            {
                category.Color = CustomColor.White;
            }

            var selectedCategory = Categories.FirstOrDefault(c => c.CategoryID == categoryId);
            if (selectedCategory != null)
            {
                selectedCategory.Color = CustomColor.Orange;
            }

            var products = await _productService.GetProductsByCategory(categoryId);

            List<int> orderedProductIds = null;
            var orderDetails = await _orderDetailservice.GetOrderDetailsByOrderId(Session.CurrentOrderID);
            if (orderDetails != null)
            {
                orderedProductIds = orderDetails.Select(orderDetail => orderDetail.ProductID).ToList();
            }

            if (orderedProductIds != null)
            {
                foreach (var product in products)
                {
                    product.IsOrdered = orderedProductIds.Contains(product.ProductID);
                }
            }
            else
            {

                foreach (var product in products)
                {
                    product.IsOrdered = false;
                }
            }

            Products = new ObservableCollection<ProductDTO>(products);
        }



        private async Task LoadOrderedItemsAsync()
        {
            DynamicGrid = true;
            OrderDetails.Clear();
            await LoadOrderDetailsFromServerAsync();
        }
        private async Task LoadOrderDetailsFromServerAsync()
        {
            
            var orderDetails = await _orderDetailservice.GetOrderDetailsByOrderId(Session.CurrentOrderID);
            
            if (orderDetails != null)
            {
                foreach(var order in orderDetails)
                {
                    order.IsOrdered = true;
                }
                OrderDetails = new ObservableCollection<OrderDetailDTO>(orderDetails);
            }
            
        }

        private void ShowDetails()
        {
            IsOverlay = true;
        }
        
        public void OnTapOutside()
        {
            IsOverlay = false;
        }

        private async void Exit()
        {
            await NavigationService.Navigation.PopModalAsync();

        }

        private async Task AddToOrderAsync(ProductDTO product)
        {
            if(Session.OrderDetails == null)
            {
                Session.OrderDetails = new List<OrderDetailDTO>();
            }
            var existingOrderDetail = Session.OrderDetails.FirstOrDefault(od => od.ProductID == product.ProductID);

            if (existingOrderDetail != null)
            {

                existingOrderDetail.Quantity++;
            }
            else
            {
                
                OrderDetailDTO orderDetail = new OrderDetailDTO
                {
                    OrderID = Session.CurrentOrderID,
                    OrderDetailID = OrderDetails.Count + 1,
                    ProductID = product.ProductID,
                    ProductName = product.ProductName,
                    Quantity = 1,
                    UnitePrice = product.Price
                };

                Session.OrderDetails.Add(orderDetail);
                UpdateOrderDetails();
                await App.Current.MainPage.DisplayAlert("Thông báo", "Thêm món thành công.", "OK");
            

            }
        }



        private async void IncreaseQuantityInOrderDetails(OrderDetailDTO orderDetail)
        {
  
            var foundOrderDetail = Session.OrderDetails.FirstOrDefault(od => od.OrderDetailID == orderDetail.OrderDetailID);
            if (foundOrderDetail != null)
            {
                foundOrderDetail.Quantity++;
            }
            UpdateOrderDetails();
        }

        private async void DecreaseQuantityInOrderDetails(OrderDetailDTO orderDetail)
        {
            var foundOrderDetail = OrderDetails.FirstOrDefault(od => od.OrderDetailID == orderDetail.OrderDetailID);
            if (foundOrderDetail != null && foundOrderDetail.Quantity > 1)
            {
                foundOrderDetail.Quantity--;
            }
            else
            {
                Session.OrderDetails.Remove(foundOrderDetail);
              
            }
            UpdateOrderDetails();
        }

        private async Task SendOrderDetail()
        {
            if (Session.OrderDetails.Count == 0)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", "Đơn hàng trống", "Đóng");
                return;
            }

            if (Session.OrderDetails.Count > 0)
            {
                bool allSuccess = true;
                foreach (var orderDetail in Session.OrderDetails)
                {
                    OrderDetailDTO success = await _orderDetailservice.AddOrderDetail(orderDetail);
                    if (success == null)
                    {
                        allSuccess = false;
                        break;
                    }
                }

                if (allSuccess)
                {
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Đặt món thành công.", "Đóng");
                    foreach (var product in Products)
                    {
                        if (product.IsOrdered) continue;
                        product.IsOrdered = Session.OrderDetails.Any(od => od.ProductID == product.ProductID);
                    }
                    Session.OrderDetails.Clear();
                    UpdateOrderDetails();
                    UpdateTotalAmount();

               
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Đặt món không thành công.", "Đóng");
                }
            }
        }


        private async Task UpdateTotalAmount()
        {
            try
            {
                TotalAmount = await _orderService.GetTotalAmount(Session.CurrentOrderID);
            
            }
            catch (Exception ex)
            {
                TotalAmount = 0;
            }
        }

    }
}
