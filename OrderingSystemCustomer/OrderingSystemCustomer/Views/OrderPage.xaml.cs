using OrderingSystemCustomer.ViewModels;

namespace OrderingSystemCustomer.Views;

public partial class OrderPage : ContentPage
{
    OrderViewModel _viewModel;
    public OrderPage()
	{
		InitializeComponent();
        _viewModel = new OrderViewModel();
        BindingContext = _viewModel; 
    }
  
}