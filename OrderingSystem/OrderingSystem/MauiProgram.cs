using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using OrderingSystem.ViewModels;
using OrderingSystem.Views.Admin.Category;
using OrderingSystem.Views.Admin.Employee;
using OrderingSystem.Views.Admin.Order;
using OrderingSystem.Views.Admin.Product;
using OrderingSystem.Views.Admin.Table;
using OrderingSystemService;

namespace OrderingSystem
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            // UseMauiApp<T>() should come before UseMauiCommunityToolkit()
            builder.UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            Routing.RegisterRoute(nameof(AddEditCategoryPage), typeof(AddEditCategoryPage));
            Routing.RegisterRoute(nameof(AddEditProductPage), typeof(AddEditProductPage));
            Routing.RegisterRoute(nameof(AddEditTablePage), typeof(AddEditTablePage));
            Routing.RegisterRoute(nameof(AddEditEmployeePage), typeof(AddEditEmployeePage));
            Routing.RegisterRoute(nameof(OrderDetailPage), typeof(OrderDetailPage));
            Routing.RegisterRoute(nameof(ChangeStatusPage), typeof(ChangeStatusPage));
            builder.Logging.AddDebug();

            builder.Services.AddTransient<ICategoryService, CategoryService>();
            builder.Services.AddTransient<IProductService, ProductService>();
            builder.Services.AddTransient<ITableService, TableService>();
            builder.Services.AddTransient<IEmployeeService, EmployeeService>();
            builder.Services.AddTransient<IOrderService, OrderService>();
            builder.Services.AddTransient<IOrderDetailService, OrderDetailService>();
        
            builder.Services.AddTransient<AddEditCategoryPage>();
            builder.Services.AddTransient<CategoryPage>();
            builder.Services.AddTransient<ProductPage>();
            builder.Services.AddTransient<TablePage>();
            builder.Services.AddTransient<AddEditProductPage>();
            builder.Services.AddTransient<AddEditTablePage>();
            builder.Services.AddTransient<EmployeePage>();
            builder.Services.AddTransient<AddEditEmployeePage>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<AddCategoryViewModel>();
            builder.Services.AddTransient<CategoryViewModel>();
            builder.Services.AddTransient<ProductViewModel>();
            builder.Services.AddTransient<AddEditProductViewModel>();
            builder.Services.AddTransient<TableViewModel>();
            builder.Services.AddTransient<AddEditTableViewModel>();
            builder.Services.AddTransient<EmployeeViewModel>();
            builder.Services.AddTransient<AddEditEmployeeViewModel>();
            builder.Services.AddTransient<AddEditEmployeePage>();
            builder.Services.AddTransient<OrderPage>();
            builder.Services.AddTransient<OrderViewModel>();
            builder.Services.AddTransient<OrderDetailViewModel>();
            builder.Services.AddTransient<OrderDetailPage>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddTransient<ChangeStatusPage>();
            builder.Services.AddTransient<ChangeStatusViewModel>();
            return builder.Build();
        }
    }
}
