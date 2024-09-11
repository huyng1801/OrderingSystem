using OrderingSystemCustomer.Utils;
using OrderingSystemCustomer.Views;
using OrderingSystemCustomerDTO;
using OrderingSystemCustomerService;

using System.Windows.Input;

namespace OrderingSystemCustomer.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly EmployeeService _employeeService;
        
        private string _employeeID = "huynguyen";
        private string _password = "huy123";

        public ICommand LoginCommand { get; private set; }

        public MainPageViewModel()
        {
            _employeeService = new EmployeeService();
            LoginCommand = new Command(async () => await LoginAsync());
        }

        public string EmployeeID
        {
            get { return _employeeID; }
            set { SetProperty(ref _employeeID, value); }
        }

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        private async Task LoginAsync()
        {
            if (string.IsNullOrEmpty(EmployeeID))
            {
                await Application.Current.MainPage.DisplayAlert("Thông báo", "Mã nhân viên không được để trống!", "Đóng");
                return;
            }
            if  (string.IsNullOrEmpty(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Thông báo", "Tên nhân viên không được để trống!", "Đóng");
                return;
            }

            LoginDTO loginDTO = new LoginDTO()
            {
                EmployeeID = EmployeeID,
                Password = Password
            };

            Session.Employee = await _employeeService.Login(loginDTO);

            if (Session.Employee != null)
            {
                await NavigationService.Navigation.PushModalAsync(new TablePage());
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Thông báo", "Thông tin đăng nhập không chính xác!", "Đóng");
            }
        }
    }
}
