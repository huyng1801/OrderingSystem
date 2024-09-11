using OrderingSystemDTO;
using OrderingSystemService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;


namespace OrderingSystem.ViewModels
{
    public class AddEditEmployeeViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly IEmployeeService _employeeService;
        private EmployeeDTO _employee;
        private bool _isNew;
        private string _password;
        public EmployeeDTO Employee
        {
            get => _employee;
            set => SetProperty(ref _employee, value);
        }
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        public bool IsNew
        {
            get => _isNew;
            set => SetProperty(ref _isNew, value);
        }

        public bool IsEditing => !IsNew;

        public string Title => IsNew ? "Thêm nhân viên" : "Sửa nhân viên";

        public ICommand SaveCommand { get; }

        public AddEditEmployeeViewModel(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
            Employee = new EmployeeDTO(); 
            IsNew = true; 

            SaveCommand = new Command(async () => await ExecuteSaveCommand());
        }

    

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("EmployeeToEdit"))
            {
                var employeeToEdit = query["EmployeeToEdit"] as EmployeeDTO;
                Employee = employeeToEdit;
                IsNew = false; 
            }
        }

        private async Task ExecuteSaveCommand()
        {
            try
            {
                if (IsNew)
                {
                    var addedEmployee = await _employeeService.AddEmployee(Employee, Password);
                    if (addedEmployee != null)
                    {
                        await Shell.Current.GoToAsync("..");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "Thêm nhân viên không thành công", "OK");
                    }
                }
                else
                {
                    var updated = await _employeeService.UpdateEmployee(Employee, Password);
                    if (updated != null)
                    {
                        await Shell.Current.GoToAsync("..");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "Cập nhật nhân viên không thành công", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
