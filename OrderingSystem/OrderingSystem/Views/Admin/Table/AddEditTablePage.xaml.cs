using OrderingSystem.ViewModels;

namespace OrderingSystem.Views.Admin.Table;

public partial class AddEditTablePage : ContentPage
{
	public AddEditTablePage(AddEditTableViewModel _viewModel)
	{
		InitializeComponent();
        BindingContext = _viewModel;
    }
}