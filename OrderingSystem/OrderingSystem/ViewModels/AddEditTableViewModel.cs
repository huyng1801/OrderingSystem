using OrderingSystemDTO;
using OrderingSystemService;
using System;
using System.Threading.Tasks;
using System.Windows.Input;


namespace OrderingSystem.ViewModels
{
    public class AddEditTableViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly ITableService _tableService;
        private TableDTO _table;
        private bool _isNew;

        public TableDTO Table
        {
            get => _table;
            set => SetProperty(ref _table, value);
        }

        public bool IsNew
        {
            get => _isNew;
            set => SetProperty(ref _isNew, value);
        }

        public bool IsEditing => !IsNew;

        public string Title => IsNew ? "Thêm bàn ăn" : "Sửa bàn ăn";

        public ICommand SaveCommand { get; }

        public AddEditTableViewModel(ITableService tableService)
        {
            _tableService = tableService;
            Table = new TableDTO(); 
            IsNew = true; 

            SaveCommand = new Command(async () => await ExecuteSaveCommand());
        }

        public async Task Initialize(TableDTO tableToEdit)
        {
            if (tableToEdit != null)
            {
                Table = tableToEdit;
                IsNew = false; 
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("TableToEdit"))
            {
                var tableToEdit = query["TableToEdit"] as TableDTO;
                Initialize(tableToEdit);
            }
        }

        private async Task ExecuteSaveCommand()
        {
            try
            {
                if (IsNew)
                {
                    var addedTable = await _tableService.AddTable(Table);
                    if (addedTable != null)
                    {
                        await Shell.Current.GoToAsync("..");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "Failed to add table", "OK");
                    }
                }
                else
                {
                    var updated = await _tableService.UpdateTable(Table.TableID, Table);
                    if (updated)
                    {
                        await Shell.Current.GoToAsync("..");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "Failed to update table", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
