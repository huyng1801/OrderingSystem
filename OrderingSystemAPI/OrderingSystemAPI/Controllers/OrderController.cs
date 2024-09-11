using Microsoft.AspNetCore.Mvc;
using OrderingSystemDTO;
using OrderingSystemService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderingSystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var orders = await _orderService.GetAllOrders();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                var order = await _orderService.GetOrderById(id);
                if (order == null)
                {
                    return NotFound();
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddOrder(OrderDTO orderDTO)
        {
            try
            {
                var addedOrder = await _orderService.AddOrder(orderDTO);
                return CreatedAtAction(nameof(GetOrderById), new { id = addedOrder.OrderID }, addedOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateOrder(int id, OrderDTO orderDTO)
        {
            try
            {
                var updatedOrder = await _orderService.UpdateOrder(id, orderDTO);
                return Ok(updatedOrder);
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

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                var result = await _orderService.DeleteOrder(id);
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
        [HttpGet("totalamount/{id}")]
        public async Task<IActionResult> CalculateTotalAmount(int id)
        {
            try
            {
                var totalAmount = await _orderService.CalculateTotalAmount(id);
                return Ok(totalAmount);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }
        [HttpGet("revenue/day/{date}")]
        public async Task<IActionResult> GetRevenueWithDay(DateTime date)
        {
            try
            {
                var revenue = await _orderService.GetRevenueWithDay(date);
                return Ok(revenue);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        [HttpGet("revenue/month/{year}/{month}")]
        public async Task<IActionResult> GetRevenueWithMonth(int year, int month)
        {
            try
            {
                var revenue = await _orderService.GetRevenueWithMonth(year, month);
                return Ok(revenue);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        [HttpGet("bestseller")]
        public async Task<IActionResult> GetTopBestSellingProducts()
        {
            try
            {
                var bestSellers = await _orderService.GetTopBestSellingProducts();
                return Ok(bestSellers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }


    }
}
