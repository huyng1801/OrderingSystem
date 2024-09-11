using OrderingSystem.ViewModels;

namespace OrderingSystem.Views.Admin.Order;

public partial class OrderDetailPage : ContentPage
{

    public OrderDetailPage(OrderDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

    }


}