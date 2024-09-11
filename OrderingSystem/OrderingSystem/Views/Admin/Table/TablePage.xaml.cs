using OrderingSystem.ViewModels;

namespace OrderingSystem.Views.Admin.Table;

public partial class TablePage : ContentPage
{
    private readonly TableViewModel viewModel;

    public TablePage(TableViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = this.viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.LoadTables();
    }
}