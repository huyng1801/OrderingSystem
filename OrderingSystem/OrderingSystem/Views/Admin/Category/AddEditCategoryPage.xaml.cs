using OrderingSystem.ViewModels;
using OrderingSystemDTO;

namespace OrderingSystem.Views.Admin.Category;

public partial class AddEditCategoryPage : ContentPage
{
 
    public AddEditCategoryPage(AddCategoryViewModel _viewModel)
	{
		InitializeComponent();

        BindingContext = _viewModel;
        
    }
}