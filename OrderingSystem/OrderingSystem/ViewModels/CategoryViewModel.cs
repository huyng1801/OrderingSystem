
using OrderingSystem.Views.Admin.Category;

using OrderingSystemDTO;
using OrderingSystemService;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;


namespace OrderingSystem.ViewModels
{
    public class CategoryViewModel : BaseViewModel
    {
        private readonly ICategoryService categoryService;
        
        private ObservableCollection<CategoryDTO> _categories;
    
        public ICommand AddCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        public CategoryViewModel(ICategoryService categoryService)
        {
            this.categoryService = categoryService;

            Categories = new ObservableCollection<CategoryDTO>();

            AddCommand = new Command(async () => await AddCategory());
            EditCommand = new Command<CategoryDTO>(async (category) => await EditCategory(category));
            DeleteCommand = new Command<CategoryDTO>(async (category) => await DeleteCategory(category));
  
            LoadCategories();
        }

        public ObservableCollection<CategoryDTO> Categories
        {
            get { return _categories; }
            set
            {
                SetProperty(ref _categories, value);
            }
        }


        public async void LoadCategories()
        {
            try
            {
                var categories = await categoryService.GetAllCategories();
                Categories = new ObservableCollection<CategoryDTO>(categories);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
            }
        }

        private async Task AddCategory()
        {
            try
            {
                await Shell.Current.GoToAsync(nameof(AddEditCategoryPage)); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
            }
        }

        private async Task EditCategory(CategoryDTO category)
        {
            try
            {
                var navigationParameter = new Dictionary<string, object>
                    {
                        { "CategoryToEdit", category }
                    };
                await Shell.Current.GoToAsync($"{nameof(AddEditCategoryPage)}", navigationParameter);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Lỗi: {ex.Message}");
            }
        }

        private async Task DeleteCategory(CategoryDTO category)
        {
            bool answer = await App.Current.MainPage.DisplayAlert("Xác nhận", $"Bạn có chắc chắn muốn xóa danh mục '{category.CategoryName}' không?", "Đồng ý", "Hủy");
            if (answer)
            {
                try
                {
                    bool result = await categoryService.DeleteCategory(category.CategoryID);
                    if (result)
                    {
                        Categories.Remove(category);
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Xác nhận", $"Không thể xóa danh mục có mã {category.CategoryID}", "Đóng");
                    
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
