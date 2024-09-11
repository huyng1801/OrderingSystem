using OrderingSystem.ViewModels;

namespace OrderingSystem.Views.Admin.Category;

public partial class CategoryPage : ContentPage
{
    private readonly CategoryViewModel viewModel;
    public CategoryPage(CategoryViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        this.viewModel = viewModel;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.LoadCategories();
    }
}