using OrderingSystemCustomerDTO;

using System.Net.Http.Json;


namespace OrderingSystemCustomerService
{
    public class OrderService
    {
        private readonly HttpClient _httpClient;
        private string BaseUrl = $"{Utils.BaseAddress}/Order";

        public OrderService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<OrderDTO>> GetAllOrders()
        {
            var response = await _httpClient.GetFromJsonAsync<List<OrderDTO>>(BaseUrl);
            return response;
        }

        public async Task<OrderDTO> GetOrderById(int id)
        {
            var url = $"{BaseUrl}/{id}";
            var response = await _httpClient.GetFromJsonAsync<OrderDTO>(url);
            return response;
        }

        public async Task<OrderDTO> AddOrder(OrderDTO orderDTO)
        {
            var response = await _httpClient.PostAsJsonAsync(BaseUrl + "/add", orderDTO);
            response.EnsureSuccessStatusCode(); 
            return await response.Content.ReadFromJsonAsync<OrderDTO>();
        }

        public async Task<OrderDTO> UpdateOrder(int id, OrderDTO orderDTO)
        {
            var url = $"{BaseUrl}/update/{id}";
            var response = await _httpClient.PutAsJsonAsync(url, orderDTO);
            response.EnsureSuccessStatusCode(); 
            return await response.Content.ReadFromJsonAsync<OrderDTO>();
        }

        public async Task<bool> DeleteOrder(int id)
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/delete/{id}");
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
        public async Task<long> GetTotalAmount(int id)
        {
            var url = $"{BaseUrl}/totalamount/{id}";
            var response = await _httpClient.GetFromJsonAsync<long>(url);
            return response;
        }

    }
}
