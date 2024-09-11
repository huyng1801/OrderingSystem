using OrderingSystem.Views.Admin.Category;
using OrderingSystemDTO;
using OrderingSystemService;
using System;
using System.Threading.Tasks;
using System.Windows.Input;



namespace OrderingSystem.ViewModels
{
   
    public class AddCategoryViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly ICategoryService categoryService;

        public  CategoryDTO _categoryToEdit; 
        private string _titleCategory;
        private string _categoryName;
        public ICommand SaveCommand { get; }

        
 
        public AddCategoryViewModel(ICategoryService categoryService)
        {
            TitleCategory = "Thêm danh mục";
            this.categoryService = categoryService;
            SaveCommand = new Command(async () => await ExecuteSaveCommand());
        }

        public string TitleCategory
        {
            get { return _titleCategory; }
            set { SetProperty(ref _titleCategory, value); }
        }
        public string CategoryName
        {
            get { return _categoryName; }
            set { SetProperty(ref _categoryName, value); }
        }


        private async Task ExecuteSaveCommand()
        {
            try
            {
                if (_categoryToEdit == null) 
                {
                    CategoryDTO newCategory = new CategoryDTO
                    {
                        CategoryName = CategoryName
                    };

                    var addedCategory = await categoryService.AddCategoryAsync(newCategory);

                    if (addedCategory != null)
                    {
                        await Shell.Current.GoToAsync("..");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Thông báo", "Thêm danh mục không thành công.", "Đóng");
                    }
                }
                else 
                {
                    _categoryToEdit.CategoryName = CategoryName;
                    var updatedCategory = await categoryService.UpdateCategory(_categoryToEdit.CategoryID, _categoryToEdit);

                    if (updatedCategory != null)
                    { 
                        await Shell.Current.GoToAsync("..");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Thông báo", "Cập nhật danh mục thất bại.", "Đóng");
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
            if (query.ContainsKey("CategoryToEdit"))
            {
                _categoryToEdit = query["CategoryToEdit"] as CategoryDTO;
                TitleCategory = "Sửa danh mục";
                CategoryName = _categoryToEdit.CategoryName;
  
            }
        }
    }
}
