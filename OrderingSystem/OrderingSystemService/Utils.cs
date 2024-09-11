using Xamarin.Essentials;
namespace OrderingSystemService
{
    public static class Utils
    {
        public static string BaseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5167/api" : "http://localhost:5167/api";
    }
}
