using Microsoft.AspNetCore.Mvc;
using OrderingSystemDTO;
using OrderingSystemAPI.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderingSystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TableController : ControllerBase
    {
        private readonly TableService _TableService;

        public TableController(TableService TableService)
        {
            _TableService = TableService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTables()
        {
            try
            {
                var tables = await _TableService.GetAllTables();
                return Ok(tables);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTableById(string id)
        {
            try
            {
                var table = await _TableService.GetTableById(id);
                if (table == null)
                {
                    return NotFound();
                }
                return Ok(table);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddTable(TableDTO tableDTO)
        {
            try
            {
                var addedTable = await _TableService.AddTable(tableDTO);
                return Ok(addedTable);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }
        [HttpPut("{id}/update")]
        public async Task<IActionResult> UpdateTableOccupiedStatus(string id, [FromBody] TableDTO tableDTO)
        {
            try
            {
                var result = await _TableService.UpdateTableOccupiedStatus(id, tableDTO);
                if (!result)
                {
                    return NotFound();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }



        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteTable(string id)
        {
            try
            {
                var result = await _TableService.DeleteTable(id);
                if (!result)
                {
                    return NotFound();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }
    }
}
