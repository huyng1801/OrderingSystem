using Microsoft.Extensions.Logging;

namespace OrderingSystemCustomer
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            Routing.RegisterRoute("MainPage", typeof(Views.MainPage));
            Routing.RegisterRoute("TablePage", typeof(Views.TablePage));
            Routing.RegisterRoute("LoginPage", typeof(Views.OrderPage));
            
            
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
