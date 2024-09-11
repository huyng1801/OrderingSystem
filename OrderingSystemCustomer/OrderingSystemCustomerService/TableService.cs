using OrderingSystemCustomerDTO;
using System.Net.Http.Json;


namespace OrderingSystemCustomerService
{
    public class TableService
    {
        private readonly HttpClient _httpClient;
        private string BaseUrl = $"{Utils.BaseAddress}/Table";

        public TableService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<TableDTO>> GetAllTables()
        {
            var response = await _httpClient.GetFromJsonAsync<List<TableDTO>>(BaseUrl);
            return response;
        }

        public async Task<TableDTO> GetTableById(string id)
        {
            var url = $"{BaseUrl}/{id}";
            var response = await _httpClient.GetFromJsonAsync<TableDTO>(url);
            return response;
        }

        public async Task<TableDTO> AddTable(TableDTO tableDTO)
        {
            var url = $"{BaseUrl}/add";
            var response = await _httpClient.PostAsJsonAsync(url, tableDTO);
            response.EnsureSuccessStatusCode(); 
            return await response.Content.ReadFromJsonAsync<TableDTO>();
        }

        public async Task<bool> UpdateTable(string id, TableDTO tableDTO)
        {
            var url = $"{BaseUrl}/{id}/update";
            var response = await _httpClient.PutAsJsonAsync(url, tableDTO);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteTable(string id)
        {
            var url = $"{BaseUrl}/delete/{id}";
            var response = await _httpClient.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }
    }
}
