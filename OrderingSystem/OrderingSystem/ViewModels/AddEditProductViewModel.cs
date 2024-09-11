using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Maui;
using OrderingSystemDTO;
using OrderingSystemService;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace OrderingSystem.ViewModels
{
    public class AddEditProductViewModel: BaseViewModel, IQueryAttributable
    {
        private readonly IProductService productService;
        private readonly ICategoryService categoryService;

        private ProductDTO productToEdit;
        private string titleProduct;
        private string productName;
        private string description;
        private long price;
        private CategoryDTO _selectedCategory;
        private IFormFile selectedImageFile;


        private ObservableCollection<CategoryDTO> categories;
    
        public ICommand SaveCommand { get; }
        public ICommand SelectImageCommand { get; }

        public AddEditProductViewModel(IProductService productService, ICategoryService categoryService)
        {
            TitleProduct = "Thêm món ăn";
            this.categoryService = categoryService;
            this.productService = productService;
            SaveCommand = new Command(async () => await ExecuteSaveCommand());
            SelectImageCommand = new Command(async () => await SelectImage());
            LoadCategories();
        }
        public string TitleProduct
        {
            get { return titleProduct; }
            set { SetProperty(ref titleProduct, value); }
        }
        private string image;
        public string Image
        {
            get { return image; }
            set { SetProperty(ref image, value); }
        }
        public string ProductName
        {
            get { return productName; }
            set { SetProperty(ref productName, value); }
        }

        public long Price
        {
            get { return price; }
            set { SetProperty(ref price, value); }
        }
        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
        }
        public ObservableCollection<CategoryDTO> Categories
        {
            get { return categories; }
            set { SetProperty(ref categories, value); }
        }
        public CategoryDTO SelectedCategory
        {
            get { return _selectedCategory; }
            set { SetProperty(ref _selectedCategory, value); }
        }
        public async void LoadCategories()
        {
            try
            {
                var categories = await categoryService.GetAllCategories();
                Categories = new ObservableCollection<CategoryDTO>(categories);
                if(productToEdit != null)
                {
                    SelectedCategory = Categories.FirstOrDefault(c => c.CategoryID == productToEdit.CategoryID);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
            }
        }

        private async Task SelectImage()
        {
            try
            {
                var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Chọn một tấm ảnh"
                });

                if (result != null)
                {
                    using (var stream = await result.OpenReadAsync())
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            await stream.CopyToAsync(ms);

                            byte[] imageData = ms.ToArray();

                            selectedImageFile = new FormFile(new MemoryStream(imageData), 0, imageData.Length, null, result.FileName);
                        }
                    }

                    Image = result.FullPath;
                }
            }
            catch (FeatureNotSupportedException)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", "MediaPicker không hỗ trợ cho thiết bị.", "Đóng");
            }
            catch (PermissionException)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", "Không đủ quyền truy cập vào Media", "Đóng");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "Đóng");
            }
        }


        private async Task ExecuteSaveCommand()
        {
            try
            {
                if (productToEdit == null)
                {
                    ProductDTO newProduct = new ProductDTO
                    {
                        ProductName = ProductName,
                        Description = Description,
                        Price = Price,
                        Image = Image,
                        CategoryID = SelectedCategory.CategoryID
                    };

                    var addedProduct = await productService.AddProduct(newProduct, selectedImageFile);

                    if (addedProduct != null)
                    {
                        await Shell.Current.GoToAsync("..");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Lỗi", "Lỗi khi thêm sản phẩm", "Đóng");
                    }
                }
                else
                {
                    productToEdit.ProductName = ProductName;
                    productToEdit.Description = Description;
                    productToEdit.Price = Price;
                    productToEdit.Image = Image;
                    productToEdit.CategoryID = SelectedCategory.CategoryID;
                    var updatedProduct = await productService.UpdateProduct(productToEdit.ProductID, productToEdit);

                    if (updatedProduct != null)
                    {
                        await Shell.Current.GoToAsync("..");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Lỗi", "Lỗi khi cập nhật sản phẩm", "Đóng");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
            }
        }



        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            try
            {
                if (query.ContainsKey("ProductToEdit"))
                {
                    productToEdit = query["ProductToEdit"] as ProductDTO;
                    TitleProduct = "Sửa món ăn";
                    ProductName = productToEdit.ProductName;
                    Description = productToEdit.Description;
                    Price = productToEdit.Price;

                    Image = productToEdit.Image;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
   
    }
}
