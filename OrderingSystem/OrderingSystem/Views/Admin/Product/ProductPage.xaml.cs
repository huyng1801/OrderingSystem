
using OrderingSystem.ViewModels;

namespace OrderingSystem.Views.Admin.Product;

public partial class ProductPage : ContentPage
{
    private readonly ProductViewModel viewModel;
    public ProductPage(ProductViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        this.viewModel = viewModel;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Get the view model from the BindingContext
        if (BindingContext is ProductViewModel viewModel)
        {
            viewModel.LoadProducts();
        }
    }
}