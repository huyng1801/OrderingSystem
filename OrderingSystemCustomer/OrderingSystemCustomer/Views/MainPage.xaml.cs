using OrderingSystemCustomer.Utils;
using OrderingSystemCustomer.ViewModels;

namespace OrderingSystemCustomer.Views
{
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel _viewModel; 

        public MainPage()
        {
            InitializeComponent();
          
            _viewModel = new MainPageViewModel(); 
            NavigationService.Navigation = this.Navigation;
            BindingContext = _viewModel;
        }
     

    }
}
