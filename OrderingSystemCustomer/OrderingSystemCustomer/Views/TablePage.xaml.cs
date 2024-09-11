using OrderingSystemCustomer.ViewModels;

namespace OrderingSystemCustomer.Views;

public partial class TablePage : ContentPage
{
    TableViewModel _viewModel;
    public TablePage()
	{
		InitializeComponent();
        _viewModel = new TableViewModel(); 
        BindingContext = _viewModel;
    }
}