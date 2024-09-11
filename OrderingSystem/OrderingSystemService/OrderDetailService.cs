using OrderingSystemDTO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace OrderingSystemService
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly string BaseUrl = $"{Utils.BaseAddress}/OrderDetail";

        public async Task<List<OrderDetailDTO>> GetOrderDetailsByOrderId(int orderId)
        {
            var url = $"{BaseUrl}/order/{orderId}";
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetFromJsonAsync<List<OrderDetailDTO>>(url);
                    return response;
                }
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<OrderDetailDTO> GetOrderDetailById(int id)
        {
            var url = $"{BaseUrl}/{id}";
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetFromJsonAsync<OrderDetailDTO>(url);
                return response;
            }
        }

        public async Task<OrderDetailDTO> AddOrderDetail(OrderDetailDTO orderDetailDTO)
        {
            var url = $"{BaseUrl}/add";
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsJsonAsync(url, orderDetailDTO);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<OrderDetailDTO>();
            }
        }

        public async Task<OrderDetailDTO> UpdateOrderDetail(int id, OrderDetailDTO orderDetailDTO)
        {
            var url = $"{BaseUrl}/update/{id}";
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PutAsJsonAsync(url, orderDetailDTO);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<OrderDetailDTO>();
            }
        }

        public async Task<bool> DeleteOrderDetail(int id)
        {
            var url = $"{BaseUrl}/delete/{id}";
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.DeleteAsync(url);
                return response.IsSuccessStatusCode;
            }
        }

        public async Task<List<OrderDetailDTO>> GetAllOrderDetails()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetFromJsonAsync<List<OrderDetailDTO>>(BaseUrl);
                return response;
            }
        }
    }

    public interface IOrderDetailService
    {
        Task<List<OrderDetailDTO>> GetOrderDetailsByOrderId(int orderId);
        Task<OrderDetailDTO> GetOrderDetailById(int id);
        Task<OrderDetailDTO> AddOrderDetail(OrderDetailDTO orderDetailDTO);
        Task<OrderDetailDTO> UpdateOrderDetail(int id, OrderDetailDTO orderDetailDTO);
        Task<bool> DeleteOrderDetail(int id);
        Task<List<OrderDetailDTO>> GetAllOrderDetails();
    }
}
