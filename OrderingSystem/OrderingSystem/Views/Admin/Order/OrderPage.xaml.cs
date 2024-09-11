using OrderingSystem.ViewModels;

namespace OrderingSystem.Views.Admin.Order;

public partial class OrderPage : ContentPage
{
    private readonly OrderViewModel viewModel;
    public OrderPage(OrderViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        this.viewModel = viewModel;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.LoadOrders();
    }
}