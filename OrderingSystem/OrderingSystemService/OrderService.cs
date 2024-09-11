using OrderingSystemDTO;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace OrderingSystemService
{
    public class OrderService : IOrderService
    {
        private readonly string BaseUrl = $"{Utils.BaseAddress}/Order";

        public async Task<List<OrderDTO>> GetAllOrders()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetFromJsonAsync<List<OrderDTO>>(BaseUrl);
                return response;
            }
        }

        public async Task<OrderDTO> GetOrderById(int id)
        {
            var url = $"{BaseUrl}/{id}";
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetFromJsonAsync<OrderDTO>(url);
                return response;
            }
        }

        public async Task<OrderDTO> AddOrder(OrderDTO orderDTO)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsJsonAsync(BaseUrl + "/add", orderDTO);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<OrderDTO>();
            }
        }

        public async Task<OrderDTO> UpdateOrder(int id, OrderDTO orderDTO)
        {
            var url = $"{BaseUrl}/update/{id}";
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PutAsJsonAsync(url, orderDTO);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<OrderDTO>();
            }
        }

        public async Task<bool> DeleteOrder(int id)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.DeleteAsync($"{BaseUrl}/delete/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return false;
                }
                else
                {
                    response.EnsureSuccessStatusCode();
                    return false;
                }
            }
        }
        public async Task<long> GetRevenueWithDay(DateTime date)
        {
            using (var httpClient = new HttpClient())
            {
                var formattedDate = date.ToString("yyyy-MM-dd");
                var response = await httpClient.GetAsync($"{BaseUrl}/revenue/day/{formattedDate}");
                response.EnsureSuccessStatusCode();

                // Read the content as a string
                var content = await response.Content.ReadAsStringAsync();

                // Parse the string to a long
                return long.Parse(content);
            }
        }

        public async Task<long> GetRevenueWithMonth(int year, int month)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{BaseUrl}/revenue/month/{year}/{month}");
                response.EnsureSuccessStatusCode();

                // Read the content as a string
                var content = await response.Content.ReadAsStringAsync();

                // Parse the string to a long
                return long.Parse(content);
            }
        }


        public async Task<List<ProductDTO>> GetTopBestSellingProducts()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{BaseUrl}/bestseller");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<ProductDTO>>();
            }
        }
    }

    public interface IOrderService
    {
        Task<List<OrderDTO>> GetAllOrders();
        Task<OrderDTO> GetOrderById(int id);
        Task<OrderDTO> AddOrder(OrderDTO orderDTO);
        Task<OrderDTO> UpdateOrder(int id, OrderDTO orderDTO);
        Task<bool> DeleteOrder(int id);
        Task<long> GetRevenueWithDay(DateTime date);
        Task<long> GetRevenueWithMonth(int year, int month);
        Task<List<ProductDTO>> GetTopBestSellingProducts();
    }
}
