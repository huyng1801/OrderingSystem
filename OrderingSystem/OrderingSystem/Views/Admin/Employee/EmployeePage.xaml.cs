using OrderingSystem.ViewModels;

namespace OrderingSystem.Views.Admin.Employee;

public partial class EmployeePage : ContentPage
{
    private readonly EmployeeViewModel viewModel;
    public EmployeePage(EmployeeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        this.viewModel = viewModel;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.LoadEmployees();
    }
}