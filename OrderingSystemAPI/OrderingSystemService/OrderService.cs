using Microsoft.EntityFrameworkCore;
using OrderingSystemData.Models;
using OrderingSystemDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystemService
{
    public class OrderService
    {
        private readonly OrderingSystemContext _context;

        public OrderService(OrderingSystemContext context)
        {
            _context = context;
        }

        public async Task<List<OrderDTO>> GetAllOrders()
        {
            var orders = await _context.Orders.ToListAsync();
            return orders.Select(o => new OrderDTO
            {
                OrderID = o.OrderID,
                OrderDate = o.OrderDate,
                Status = o.Status,
                EmployeeID = o.EmployeeID,
                TableID = o.TableID
            }).OrderByDescending(o => o.OrderDate).ToList();
        }
        public async Task<long> GetRevenueWithDay(DateTime day)
        {
            var orders = await _context.Orders
                                        .Where(o => o.OrderDate.Date == day.Date)
                                        .Include(o => o.OrderDetails)
                                        .ToListAsync();

            long totalRevenue = 0;

            foreach (var order in orders)
            {
                foreach (var orderDetail in order.OrderDetails)
                {
                    totalRevenue += orderDetail.UnitePrice * orderDetail.Quantity;
                }
            }

            return totalRevenue;
        }
        public async Task<long> GetRevenueWithMonth(int year, int month)
        {
            var orders = await _context.Orders
                                        .Where(o => o.OrderDate.Year == year && o.OrderDate.Month == month)
                                        .Include(o => o.OrderDetails)
                                        .ToListAsync();

            long totalRevenue = 0;

            foreach (var order in orders)
            {
                foreach (var orderDetail in order.OrderDetails)
                {
                    totalRevenue += orderDetail.UnitePrice * orderDetail.Quantity;
                }
            }

            return totalRevenue;
        }
        public async Task<List<ProductDTO>> GetTopBestSellingProducts()
        {
            var bestSellingProducts = await _context.OrderDetails
                .GroupBy(od => od.ProductID)
                .Select(g => new { ProductID = g.Key, TotalQuantity = g.Sum(od => od.Quantity) })
                .OrderByDescending(x => x.TotalQuantity)
                .Take(10)
                .ToListAsync();

            var productDTOs = new List<ProductDTO>();

            foreach (var bestSellerProduct in bestSellingProducts)
            {
                var product = await _context.Products.FindAsync(bestSellerProduct.ProductID);
                if (product != null)
                {
                    // Find category name
                    var categoryName = await _context.Categories
                        .Where(c => c.CategoryID == product.CategoryID)
                        .Select(c => c.CategoryName)
                        .FirstOrDefaultAsync();

                    productDTOs.Add(new ProductDTO
                    {
                        ProductID = product.ProductID,
                        ProductName = product.ProductName,
                        Description = product.Description,
                        Price = product.Price,
                        Image = product.Image,
                        CategoryID = product.CategoryID,
                        CategoryName = categoryName
                    });
                }
            }

            return productDTOs;
        }



        public async Task<OrderDTO> GetOrderById(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                return null;

            return new OrderDTO
            {
                OrderID = order.OrderID,
                OrderDate = order.OrderDate,
                Status = order.Status,
                EmployeeID = order.EmployeeID,
                TableID = order.TableID
            };
        }
        public async Task<OrderDTO> AddOrder(OrderDTO orderDTO)
        {
            var tableExists = await _context.Tables.AnyAsync(t => t.TableID == orderDTO.TableID);
            if (!tableExists)
            {
                throw new InvalidOperationException("Mã bàn không tồn tại");
            }

            var order = new Order
            {
                OrderDate = orderDTO.OrderDate ?? DateTime.Now,
                Status = orderDTO.Status,
                EmployeeID = orderDTO.EmployeeID,
                TableID = orderDTO.TableID
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            orderDTO.OrderID = order.OrderID;
            return orderDTO;
        }

        public async Task<OrderDTO> UpdateOrder(int orderId, OrderDTO orderDTO)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                throw new KeyNotFoundException($"Đơn hàng với ID {orderId} không tìm thấy");
            var tableExists = await _context.Tables.AnyAsync(t => t.TableID == orderDTO.TableID);
            if (!tableExists)
            {
                throw new InvalidOperationException("Mã bàn không tồn tại");
            }
            order.OrderDate = orderDTO.OrderDate ?? DateTime.Now;
            order.Status = orderDTO.Status;
            order.EmployeeID = orderDTO.EmployeeID;
            order.TableID = orderDTO.TableID;

            await _context.SaveChangesAsync();
            return orderDTO;
        }

        public async Task<bool> DeleteOrder(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                return false;

            bool hasOrderDetails = await _context.OrderDetails.AnyAsync(od => od.OrderID == orderId);
            if (hasOrderDetails)
            {
                return false;
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<long> CalculateTotalAmount(int orderId)
        {
            var orderDetails = await _context.OrderDetails
                                            .Where(od => od.OrderID == orderId)
                                            .ToListAsync();

            long totalAmount = 0;

            foreach (var orderDetail in orderDetails)
            {
                totalAmount += orderDetail.UnitePrice * orderDetail.Quantity;
            }

            return totalAmount;
        }

    }
}
