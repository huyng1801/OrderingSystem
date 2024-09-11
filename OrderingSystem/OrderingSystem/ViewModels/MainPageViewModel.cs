using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using OrderingSystemDTO;
using OrderingSystemService;

namespace OrderingSystem.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly IOrderService _orderService;

        public MainPageViewModel(IOrderService orderService)
        {
            _orderService = orderService;
            LoadData();
        }

        private string _dayRevenue;
        public string DayRevenue
        {
            get => _dayRevenue;
            set => SetProperty(ref _dayRevenue, value);
        }

        private string _monthRevenue;
        public string MonthRevenue
        {
            get => _monthRevenue;
            set => SetProperty(ref _monthRevenue, value);
        }

        private List<ProductDTO> _bestSellingProducts;
        public List<ProductDTO> BestSellingProducts
        {
            get => _bestSellingProducts;
            set => SetProperty(ref _bestSellingProducts, value);
        }

        private async void LoadData()
        {
            // Fetch revenue for a specific day
            DateTime selectedDate = DateTime.Now; // Example: Replace with the selected date
            long dayRevenue = await _orderService.GetRevenueWithDay(selectedDate);
            DayRevenue = dayRevenue.ToString();

            // Fetch revenue for a specific month
            int selectedYear = DateTime.Now.Year; // Example: Replace with the selected year
            int selectedMonth = DateTime.Now.Month; // Example: Replace with the selected month
            long monthRevenue = await _orderService.GetRevenueWithMonth(selectedYear, selectedMonth);
            MonthRevenue = monthRevenue.ToString();

            // Fetch top best-selling products
            List<ProductDTO> bestSellingProducts = await _orderService.GetTopBestSellingProducts();
            BestSellingProducts = bestSellingProducts;
        }
    }
}
