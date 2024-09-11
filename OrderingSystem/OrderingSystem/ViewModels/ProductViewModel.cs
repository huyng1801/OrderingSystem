using OrderingSystem.Views.Admin.Product;
using OrderingSystemDTO;
using OrderingSystemService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OrderingSystem.ViewModels
{
    public class ProductViewModel : BaseViewModel
    {
        private readonly IProductService productService;
        private const int PageSize = 5; // Number of items per page
        private int currentPage = 1;
        private int totalItemsCount = 0;
        private int totalPages = 0;
        private bool _isFirstPage;
        private bool _isLastPage;

        private ObservableCollection<ProductDTO> _products;

        public ICommand AddCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand NextPageCommand { get; private set; }
        public ICommand PreviousPageCommand { get; private set; }

        public ICommand FirstPageCommand { get; private set; }
        public ICommand LastPageCommand { get; private set; }
        public ProductViewModel(IProductService productService)
        {
            this.productService = productService;

            Products = new ObservableCollection<ProductDTO>();

            AddCommand = new Command(async () => await AddProduct());
            EditCommand = new Command<ProductDTO>(async (product) => await EditProduct(product));
            DeleteCommand = new Command<ProductDTO>(async (product) => await DeleteProduct(product));
            NextPageCommand = new Command(async () => await LoadNextPage(), () => currentPage < totalPages);
            PreviousPageCommand = new Command(async () => await LoadPreviousPage(), () => currentPage > 1);
            FirstPageCommand = new Command(async () => await LoadFirstPage());
            LastPageCommand = new Command(async () => await LoadLastPage());
            LoadProducts();
        }
        public bool IsFirstPage
        {
            get { return _isFirstPage; }
            set { SetProperty(ref _isFirstPage, value); }
        }


        public bool IsLastPage
        {
            get { return _isLastPage; }
            set { SetProperty(ref _isLastPage, value); }
        }
        private async Task LoadFirstPage()
        {
            currentPage = 1;
            await LoadProducts();
        }

        private async Task LoadLastPage()
        {
            currentPage = totalPages;
            await LoadProducts();
        }
        public ObservableCollection<ProductDTO> Products
        {
            get { return _products; }
            set
            {
                SetProperty(ref _products, value);
            }
        }

        public async Task LoadProducts()
        {
            try
            {
                var allProducts = await productService.GetAllProducts();
                int startIndex = (currentPage - 1) * PageSize;
                var itemsForPage = allProducts.Skip(startIndex).Take(PageSize).ToList();
                Products = new ObservableCollection<ProductDTO>(itemsForPage);
                totalItemsCount = allProducts.Count();
                totalPages = (int)Math.Ceiling((double)totalItemsCount / PageSize);
                UpdatePageCommands();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }




        private async Task LoadNextPage()
        {
            currentPage++;
            await LoadProducts();
        }

        private async Task LoadPreviousPage()
        {
            currentPage--;
            await LoadProducts();
        }

        private void UpdatePageCommands()
        {
            ((Command)NextPageCommand).ChangeCanExecute();
            ((Command)PreviousPageCommand).ChangeCanExecute();

           if(currentPage == 1)
            {
                
                IsFirstPage = false;
                IsLastPage = true;
            }
           else if( currentPage == totalItemsCount )
            {
                IsFirstPage = true;
                IsLastPage = false;
            }
           else
            {
                IsFirstPage = true;
                IsLastPage = true;
            }
            OnPropertyChanged(nameof(CurrentPageText));
        }

        public string CurrentPageText => $"{currentPage} / {totalPages}";

        private async Task AddProduct()
        {
            try
            {
                await Shell.Current.GoToAsync(nameof(AddEditProductPage));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
            }
        }
        private async Task EditProduct(ProductDTO product)
        {
            try
            {
                var navigationParameter = new Dictionary<string, object>
                    {
                        { "ProductToEdit", product }
                    };
                await Shell.Current.GoToAsync($"{nameof(AddEditProductPage)}", navigationParameter);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Lỗi: {ex.Message}");
            }
        }

        private async Task DeleteProduct(ProductDTO product)
        {
            bool answer = await App.Current.MainPage.DisplayAlert("Xác nhân", $"Bạn có chắc chắn muốn xóa sản phẩm '{product.ProductName}' không?", "Đồng ý", "Hủy");
            if (answer)
            {
                try
                {
                    bool result = await productService.DeleteProduct(product.ProductID);
                    if (result)
                    {
                        Products.Remove(product);
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Xác nhận", $"Không thể xóa sản phẩm có mã {product.ProductID}", "Đóng");

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi: {ex.Message}");
                }
            }
        }
    }
}
