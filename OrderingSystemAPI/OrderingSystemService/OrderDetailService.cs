using Microsoft.EntityFrameworkCore;
using OrderingSystemData.Models;
using OrderingSystemDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderingSystemService
{
    public class OrderDetailService
    {
        private readonly OrderingSystemContext _context;

        public OrderDetailService(OrderingSystemContext context)
        {
            _context = context;
        }

        public async Task<List<OrderDetailDTO>> GetAllOrderDetails()
        {
            var orderDetails = await _context.OrderDetails.ToListAsync();
            return orderDetails.Select(od => new OrderDetailDTO
            {
                OrderDetailID = od.OrderDetailID,
                UnitePrice = od.UnitePrice,
                Quantity = od.Quantity,
                OrderID = od.OrderID,
                ProductID = od.ProductID,
                IsServed = od.IsServed
            }).OrderByDescending(od => od.OrderDetailID).ToList();
        }
        public async Task<List<OrderDetailDTO>> GetOrderDetailsByOrderId(int orderId)
        {
            var orderDetails = await _context.OrderDetails
                                              .Where(od => od.OrderID == orderId)
                                              .Include(od => od.Product) // Include product entity
                                              .ToListAsync();

            if (orderDetails == null || orderDetails.Count == 0)
            {
                return null;
            }

            return orderDetails.Select(od => new OrderDetailDTO
            {
                OrderDetailID = od.OrderDetailID,
                UnitePrice = od.UnitePrice,
                Quantity = od.Quantity,
                OrderID = od.OrderID,
                ProductID = od.ProductID,
                ProductName = od.Product?.ProductName, // Assign product name
                IsServed = od.IsServed
            }).ToList();
        }

        public async Task<OrderDetailDTO> GetOrderDetailById(int orderDetailId)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(orderDetailId);
            if (orderDetail == null)
            {
                return null;
            }

            var orderDetailDTO = new OrderDetailDTO
            {
                OrderDetailID = orderDetail.OrderDetailID,
                UnitePrice = orderDetail.UnitePrice,
                Quantity = orderDetail.Quantity,
                OrderID = orderDetail.OrderID,
                ProductID = orderDetail.ProductID,
                IsServed = orderDetail.IsServed
            };

            return orderDetailDTO;
        }


        public async Task<OrderDetailDTO> AddOrderDetail(OrderDetailDTO orderDetailDTO)
        {
            var orderExists = await _context.Orders.AnyAsync(o => o.OrderID == orderDetailDTO.OrderID);
            var productExists = await _context.Products.AnyAsync(p => p.ProductID == orderDetailDTO.ProductID);

            if (!orderExists || !productExists)
            {
                throw new InvalidOperationException("Mã đơn hàng hoặc mã sản phẩm không tồn tại");
            }
            var existingOrderDetail = await _context.OrderDetails.FirstOrDefaultAsync(od =>
                od.OrderID == orderDetailDTO.OrderID && od.ProductID == orderDetailDTO.ProductID);

            if (existingOrderDetail != null)
            {
                existingOrderDetail.Quantity += orderDetailDTO.Quantity;
                await _context.SaveChangesAsync();

                orderDetailDTO.OrderDetailID = existingOrderDetail.OrderDetailID;
                orderDetailDTO.Quantity = existingOrderDetail.Quantity;
            }
            else
            {
                var orderDetail = new OrderDetail
                {
                    UnitePrice = orderDetailDTO.UnitePrice,
                    Quantity = orderDetailDTO.Quantity,
                    OrderID = orderDetailDTO.OrderID,
                    ProductID = orderDetailDTO.ProductID,
                    IsServed = orderDetailDTO.IsServed
                };

                _context.OrderDetails.Add(orderDetail);
                await _context.SaveChangesAsync();

                orderDetailDTO.OrderDetailID = orderDetail.OrderDetailID;
            }

            return orderDetailDTO;
        }


        public async Task<OrderDetailDTO> UpdateOrderDetail(int orderDetailId, OrderDetailDTO orderDetailDTO)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(orderDetailId);
            if (orderDetail == null)
            {
                throw new KeyNotFoundException($"Đơn hàng với ID {orderDetailId} không tìm thấy");
            }

            var orderExists = await _context.Orders.AnyAsync(o => o.OrderID == orderDetailDTO.OrderID);
            var productExists = await _context.Products.AnyAsync(p => p.ProductID == orderDetailDTO.ProductID);

            if (!orderExists || !productExists)
            {
                throw new InvalidOperationException("Mã đơn hàng hoặc mã sản phẩm không tồn tại");
            }

            orderDetail.UnitePrice = orderDetailDTO.UnitePrice;
            orderDetail.Quantity = orderDetailDTO.Quantity;
            orderDetail.OrderID = orderDetailDTO.OrderID;
            orderDetail.ProductID = orderDetailDTO.ProductID;
            orderDetail.IsServed = orderDetailDTO.IsServed;
            await _context.SaveChangesAsync();

            return orderDetailDTO;
        }


        public async Task<bool> DeleteOrderDetail(int orderDetailId)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(orderDetailId);
            if (orderDetail == null)
            {
                return false;
            }

            _context.OrderDetails.Remove(orderDetail);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
