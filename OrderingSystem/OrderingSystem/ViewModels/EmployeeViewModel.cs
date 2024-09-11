using OrderingSystem.Views.Admin.Employee;
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
    public class EmployeeViewModel : BaseViewModel
    {
        private readonly IEmployeeService _employeeService;
        private ObservableCollection<EmployeeDTO> _employees;

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public ObservableCollection<EmployeeDTO> Employees
        {
            get => _employees;
            set => SetProperty(ref _employees, value);
        }

        public EmployeeViewModel(IEmployeeService employeeService)
        {
            _employeeService = employeeService;

            Employees = new ObservableCollection<EmployeeDTO>();

            AddCommand = new Command(async () => await AddEmployee());
            EditCommand = new Command<EmployeeDTO>(async employee => await EditEmployee(employee));
            DeleteCommand = new Command<EmployeeDTO>(async employee => await DeleteEmployee(employee));

            LoadEmployees();
        }

        public async void LoadEmployees()
        {
            try
            {
                var employees = await _employeeService.GetAllEmployees();
                Employees = new ObservableCollection<EmployeeDTO>(employees);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private async Task AddEmployee()
        {
            try
            {
                await Shell.Current.GoToAsync(nameof(AddEditEmployeePage));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private async Task EditEmployee(EmployeeDTO employee)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "EmployeeToEdit", employee }
                };
                await Shell.Current.GoToAsync($"{nameof(AddEditEmployeePage)}", parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private async Task DeleteEmployee(EmployeeDTO employee)
        {
            bool answer = await App.Current.MainPage.DisplayAlert("Confirmation", $"Are you sure you want to delete employee '{employee.EmployeeName}'?", "Yes", "No");
            if (answer)
            {
                try
                {
                    bool result = await _employeeService.DeleteEmployee(employee.EmployeeID);
                    if (result)
                    {
                        Employees.Remove(employee);
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Error", $"Failed to delete employee with ID {employee.EmployeeID}", "OK");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
}
