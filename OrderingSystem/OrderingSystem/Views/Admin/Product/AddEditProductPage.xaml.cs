using OrderingSystem.ViewModels;

namespace OrderingSystem.Views.Admin.Product;

public partial class AddEditProductPage : ContentPage
{
	public AddEditProductPage(AddEditProductViewModel _viewModel)
	{
		InitializeComponent();
		BindingContext = _viewModel;

    }
}