using Microsoft.AspNetCore.Mvc;
using OrderingSystemDTO;
using OrderingSystemService;
using System;
using System.Threading.Tasks;

namespace OrderingSystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _employeeService;

        public EmployeeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var employees = await _employeeService.GetAllEmployees();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            try
            {
           
                string employeeID = loginDTO.EmployeeID;
                string password = loginDTO.Password;

                var employeeDTO = await _employeeService.Login(employeeID, password);
                if (employeeDTO == null)
                {
                    return Unauthorized("Mã nhân viên hoặc mật khẩu không chính xác");
                }

                return Ok(employeeDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddEmployee([FromForm] EmployeeDTO employeeDTO, [FromForm] string password)
        {
            try
            {
                var addedEmployeeDTO = await _employeeService.AddEmployee(employeeDTO, password);
                return CreatedAtAction(nameof(GetEmployeeById), new { id = addedEmployeeDTO.EmployeeID }, addedEmployeeDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }


        [HttpPut("update")]
        public async Task<IActionResult> UpdateEmployee([FromForm] EmployeeDTO employeeDTO, [FromForm] string password = null)
        {
            try
            {
                var updatedEmployeeDTO = await _employeeService.UpdateEmployee(employeeDTO, password);
                return Ok(updatedEmployeeDTO);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(string id)
        {
            try
            {
                var employeeDTO = await _employeeService.GetEmployeeById(id);
                if (employeeDTO == null)
                {
                    return NotFound();
                }

                return Ok(employeeDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            try
            {
                var result = await _employeeService.DeleteEmployee(id);
                if (result)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }

    }
}
