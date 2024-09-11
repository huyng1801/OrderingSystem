using Microsoft.AspNetCore.Mvc;
using OrderingSystemDTO;
using OrderingSystemService;
using System;
using System.Threading.Tasks;

namespace OrderingSystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderDetailController : ControllerBase
    {
        private readonly OrderDetailService _orderDetailService;

        public OrderDetailController(OrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }
        // GET: api/OrderDetails/ByOrderId/{orderId}
        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetOrderDetailsByOrderId(int orderId)
        {
            var orderDetails = await _orderDetailService.GetOrderDetailsByOrderId(orderId);
        
            return Ok(orderDetails);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllOrderDetails()
        {
            try
            {
                var orderDetails = await _orderDetailService.GetAllOrderDetails();
                return Ok(orderDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderDetailById(int id)
        {
            try
            {
                var orderDetail = await _orderDetailService.GetOrderDetailById(id);
                if (orderDetail == null)
                {
                    return NotFound();
                }
                return Ok(orderDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddOrderDetail(OrderDetailDTO orderDetailDTO)
        {
            try
            {
                var addedOrderDetail = await _orderDetailService.AddOrderDetail(orderDetailDTO);
                return CreatedAtAction(nameof(GetOrderDetailById), new { id = addedOrderDetail.OrderDetailID }, addedOrderDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateOrderDetail(int id, OrderDetailDTO orderDetailDTO)
        {
            try
            {
                var updatedOrderDetail = await _orderDetailService.UpdateOrderDetail(id, orderDetailDTO);
                return Ok(updatedOrderDetail);
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
        public async Task<IActionResult> DeleteOrderDetail(int id)
        {
            try
            {
                var success = await _orderDetailService.DeleteOrderDetail(id);
                if (!success)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }
    }
}
