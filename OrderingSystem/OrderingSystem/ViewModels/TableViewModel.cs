using OrderingSystem.Views.Admin.Table;
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
    public class TableViewModel : BaseViewModel
    {
        private readonly ITableService tableService;
        private ObservableCollection<TableDTO> tables;

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public TableViewModel(ITableService tableService)
        {
            this.tableService = tableService;
            AddCommand = new Command(async () => await AddTable());
            EditCommand = new Command<TableDTO>(async (table) => await EditTable(table));
            DeleteCommand = new Command<TableDTO>(async (table) => await DeleteTable(table));
            Tables = new ObservableCollection<TableDTO>();
        }

        public ObservableCollection<TableDTO> Tables
        {
            get => tables;
            set => SetProperty(ref tables, value);
        }

        public async void LoadTables()
        {
            try
            {
                var tableList = await tableService.GetAllTables();
                Tables = new ObservableCollection<TableDTO>(tableList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        private async Task AddTable()
        {
            await Shell.Current.GoToAsync(nameof(AddEditTablePage));
        }

        // Inside EditTable method
        private async Task EditTable(TableDTO table)
        {
            try
            {
                var navigationParameter = new Dictionary<string, object>
                    {
                        { "TableToEdit", table }
                    };
                await Shell.Current.GoToAsync(nameof(AddEditTablePage), navigationParameter);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Lỗi: {ex.Message}");
            }
           
        }

        private async Task DeleteTable(TableDTO table)
        {
            try
            {
                // Hiển thị hộp thoại xác nhận trước khi xóa bàn
                bool answer = await App.Current.MainPage.DisplayAlert("Xác nhận", $"Bạn có chắc chắn muốn xóa bàn '{table.TableID}' không?", "Đồng ý", "Hủy");

                if (answer)
                {
                    // Gọi dịch vụ để xóa bàn từ cơ sở dữ liệu
                    bool result = await tableService.DeleteTable(table.TableID);

                    if (result)
                    {
                        // Xóa bàn khỏi danh sách nếu xóa thành công
                        Tables.Remove(table);
                        await App.Current.MainPage.DisplayAlert("Thông báo", $"Bàn '{table.TableID}' đã được xóa thành công.", "Đóng");
                    }
                    else
                    {
                        // Hiển thị thông báo không thể xóa nếu thất bại
                        await App.Current.MainPage.DisplayAlert("Lỗi", $"Không thể xóa bàn '{table.TableID}'.", "Đóng");
                    }
                }
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi nếu có lỗi xảy ra
                Console.WriteLine($"Lỗi: {ex.Message}");
                await App.Current.MainPage.DisplayAlert("Lỗi", $"Đã xảy ra lỗi trong quá trình xóa bàn: {ex.Message}", "Đóng");
            }
        }

    }
}
