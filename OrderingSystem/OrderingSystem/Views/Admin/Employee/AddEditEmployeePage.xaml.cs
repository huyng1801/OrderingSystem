using OrderingSystem.ViewModels;

namespace OrderingSystem.Views.Admin.Employee;

public partial class AddEditEmployeePage : ContentPage
{
	public AddEditEmployeePage(AddEditEmployeeViewModel _viewModel)
	{
		InitializeComponent();
        BindingContext = _viewModel;
    }

}