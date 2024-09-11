using OrderingSystemCustomerDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystemCustomerService
{
    public class OrderDetailService
    {
        private readonly HttpClient _httpClient;
        private string BaseUrl = $"{Utils.BaseAddress}/OrderDetail";

        public OrderDetailService()
        {
            _httpClient = new HttpClient();
        }
        public async Task<List<OrderDetailDTO>> GetOrderDetailsByOrderId(int orderId)
        {
            var url = $"{BaseUrl}/order/{orderId}";
            try
            {
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    if (response.Content.Headers.ContentLength == 0)
                    {
                  
                        return new List<OrderDetailDTO>(); 
                    }

                    return await response.Content.ReadFromJsonAsync<List<OrderDetailDTO>>();
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null; 
                }
                else
                {
               
                    response.EnsureSuccessStatusCode(); 
                    return null;
                }

            }
            catch (Exception ex)
            {
                // Handle any unexpected exceptions during the HTTP request
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }


        public async Task<OrderDetailDTO> GetOrderDetailById(int id)
        {
            var url = $"{BaseUrl}/{id}";
            var response = await _httpClient.GetFromJsonAsync<OrderDetailDTO>(url);
            return response;
        }

        public async Task<OrderDetailDTO> AddOrderDetail(OrderDetailDTO orderDetailDTO)
        {
            var url = $"{BaseUrl}/add";
            var response = await _httpClient.PostAsJsonAsync(url, orderDetailDTO);
            response.EnsureSuccessStatusCode(); 
            return await response.Content.ReadFromJsonAsync<OrderDetailDTO>();
        }

        public async Task<OrderDetailDTO> UpdateOrderDetail(int id, OrderDetailDTO orderDetailDTO)
        {
            var url = $"{BaseUrl}/update/{id}";
            var response = await _httpClient.PutAsJsonAsync(url, orderDetailDTO);
            response.EnsureSuccessStatusCode(); 
            return await response.Content.ReadFromJsonAsync<OrderDetailDTO>();
        }

        public async Task<bool> DeleteOrderDetail(int id)
        {
            var url = $"{BaseUrl}/delete/{id}";
            var response = await _httpClient.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<OrderDetailDTO>> GetAllOrderDetails()
        {
            var response = await _httpClient.GetFromJsonAsync<List<OrderDetailDTO>>(BaseUrl);
            return response;
        }
    }
}
