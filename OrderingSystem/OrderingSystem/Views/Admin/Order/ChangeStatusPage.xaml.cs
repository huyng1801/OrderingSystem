using OrderingSystem.ViewModels;

namespace OrderingSystem.Views.Admin.Order;

public partial class ChangeStatusPage : ContentPage
{
	public ChangeStatusPage(ChangeStatusViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}