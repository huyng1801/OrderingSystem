using OrderingSystemCustomerDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystemCustomerService
{
    public class EmployeeService
    {
        private readonly HttpClient _httpClient;
        private string BaseUrl = $"{Utils.BaseAddress}/Employee";

        public EmployeeService()
        {
            _httpClient = new HttpClient();
        }
        public async Task<EmployeeDTO> Login(LoginDTO loginDTO)
        {
            var url = $"{BaseUrl}/login";

    
            var response = await _httpClient.PostAsJsonAsync(url, loginDTO);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<EmployeeDTO>();
        }

        public async Task<EmployeeDTO> GetEmployeeById(string id)
        {
            var url = $"{BaseUrl}/{id}";
            var response = await _httpClient.GetFromJsonAsync<EmployeeDTO>(url);
            return response;
        }

        public async Task<EmployeeDTO> AddEmployee(EmployeeDTO employeeDTO, string password)
        {
            var url = $"{BaseUrl}/add?password={password}";
            var response = await _httpClient.PostAsJsonAsync(url, employeeDTO);
            response.EnsureSuccessStatusCode(); // Throws if HTTP response status is not success
            return await response.Content.ReadFromJsonAsync<EmployeeDTO>();
        }

        public async Task<EmployeeDTO> UpdateEmployee(EmployeeDTO employeeDTO, string password = null)
        {
            var url = $"{BaseUrl}/update";
            if (!string.IsNullOrEmpty(password))
            {
                url += $"?password={password}";
            }
            var response = await _httpClient.PutAsJsonAsync(url, employeeDTO);
            response.EnsureSuccessStatusCode(); // Throws if HTTP response status is not success
            return await response.Content.ReadFromJsonAsync<EmployeeDTO>();
        }

        public async Task<List<EmployeeDTO>> GetAllEmployees()
        {
            var response = await _httpClient.GetFromJsonAsync<List<EmployeeDTO>>(BaseUrl);
            return response;
        }
    }
}
