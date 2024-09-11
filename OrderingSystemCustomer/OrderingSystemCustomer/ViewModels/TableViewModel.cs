using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using OrderingSystemCustomer.Utils;
using OrderingSystemCustomer.Views;
using OrderingSystemCustomerDTO;
using OrderingSystemCustomerService;

namespace OrderingSystemCustomer.ViewModels
{
    public class TableViewModel : BaseViewModel
    {
        private readonly TableService _tableService;
        private readonly OrderService _orderService;
        public EmployeeDTO Employee { get; private set; }

        private string _currentTableID;
        private int _orderID;

        private ObservableCollection<TableDTO> _tables;

        public ICommand SelectTableCommand { get; }
        public ICommand CreateInvoiceCommand { get; }
        public ICommand GoToMainPageCommand { get; }

        public TableViewModel()
        {
            Employee = Session.Employee;
            CurrentTableID = Session.CurrentTableID;
            _tableService = new TableService();
            _orderService = new OrderService();
            SelectTableCommand = new Command<TableDTO>(async (table) => await SelectTableAsync(table));
            CreateInvoiceCommand = new Command(CreateInvoice);
            GoToMainPageCommand = new Command(GoToMainPage);
            LoadTables();
        }

        public int OrderID
        {
            get { return _orderID; }
            set { SetProperty(ref _orderID, value); }
        }

        public string CurrentTableID
        {
            get { return _currentTableID; }
            set { SetProperty(ref _currentTableID, value); }
        }
        public ObservableCollection<TableDTO> Tables
        {
            get { return _tables; }
            set
            {
                SetProperty(ref _tables, value);

            }
        }

        private async Task LoadTables()
        {
            var tables = await _tableService.GetAllTables();
            Tables = new ObservableCollection<TableDTO>(tables);
        }

        private async Task SelectTableAsync(TableDTO table)
        {
            if (Session.CurrentOrderID != 0)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", "Đã lập hóa đơn, không thể cập nhật.", "Đóng");
                return;
            }
            if (table != null && !table.IsOccupied && table.TableID != CurrentTableID)
            {
                var confirm = await App.Current.MainPage.DisplayAlert("Xác nhận", $"Bạn có muốn chọn bàn {table.TableID}?", "Đồng ý", "Không");
                if (confirm)
                {
                    var updatedTable = await _tableService.GetTableById(table.TableID);
                    if (updatedTable != null && !updatedTable.IsOccupied)
                    {
                        updatedTable.IsOccupied = true;
                        bool isSuccess = await _tableService.UpdateTable(updatedTable.TableID, updatedTable);
                        if (!string.IsNullOrEmpty(CurrentTableID))
                            await _tableService.UpdateTable(CurrentTableID, new TableDTO() { TableID = CurrentTableID, IsOccupied = false });
                        if (isSuccess)
                        {
                            await App.Current.MainPage.DisplayAlert("Thông báo", "Đổi trạng thái bàn thành công.", "OK");
                            CurrentTableID = updatedTable.TableID;
                            Session.CurrentTableID = CurrentTableID;
                            await LoadTables();
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Thông báo", "Đổi trạng thái bàn không thành công.", "OK");
                        }
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Thông báo", "Bàn đã được chọn hoặc không tồn tại.", "OK");
                    }
                }
            }
            else if (table != null && table.IsOccupied && table.TableID == CurrentTableID)
            {
                var confirm = await App.Current.MainPage.DisplayAlert("Xác nhận", $"Bạn có muốn hủy bàn {table.TableID}?", "Đồng ý", "Không");
                if (confirm)
                {
                    var updatedTable = await _tableService.GetTableById(table.TableID);
                    if (updatedTable != null && updatedTable.IsOccupied)
                    {
                        updatedTable.IsOccupied = false;
                        bool isSuccess = await _tableService.UpdateTable(updatedTable.TableID, updatedTable);
                        if (isSuccess)
                        {
                            await App.Current.MainPage.DisplayAlert("Thông báo", "Hủy bàn thành công.", "OK");
                            CurrentTableID = string.Empty;
                            Session.CurrentTableID = CurrentTableID;
                            await LoadTables();
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Thông báo", "Hủy bàn không thành công.", "OK");
                        }
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Thông báo", "Bàn không tồn tại.", "OK");
                    }
                }
            }
        }

        private async void CreateInvoice()
        {
            if(Session.CurrentOrderID != 0)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", "Hóa đơn đã tồn tại.", "Đóng");
                return;
            }
            if (!string.IsNullOrEmpty(Session.CurrentTableID))
            {
                OrderDTO newOrder = new OrderDTO
                {
                    OrderDate = DateTime.Now,
                    Status = "Mới tạo",
                    EmployeeID = Employee.EmployeeID,
                    EmployeeName = Employee.EmployeeName,
                    TableID = CurrentTableID
                };

                var addedOrder = await _orderService.AddOrder(newOrder);

                if (addedOrder != null)
                {
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Hóa đơn đã được tạo thành công.", "Đóng");
                    OrderID = addedOrder.OrderID;
                    Session.CurrentOrderID = OrderID;
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Lỗi", "Đã xảy ra lỗi khi tạo hóa đơn.", "Đóng");
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", "Vui lòng chọn bàn trước khi tạo hóa đơn.", "Đóng");
            }
        }

        private async void GoToMainPage()
        {
            if(!string.IsNullOrEmpty(Session.CurrentTableID) &&  Session.CurrentOrderID != 0) {
                await NavigationService.Navigation.PushModalAsync(new OrderPage());
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", "Vui lòng chọn bàn và lập hóa đơn.", "Đóng");
            }
        }
    }
}
