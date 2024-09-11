using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OrderingSystemData.Models;

namespace OrderingSystemData
{
    public static class SeedData
    {
        public static void Initialize(OrderingSystemContext context)
        {
            // Ensure the database is created
            context.Database.EnsureCreated();

            // Check if data already exists
            if (context.Categories.Any() || context.Products.Any() || context.Employees.Any() || context.Tables.Any())
            {
                return; // Data already seeded
            }

            // Seed Categories
            var categories = new Category[]
            {
                new Category { CategoryID = 1, CategoryName = "Khai vị & Ăn cùng" },
                new Category { CategoryID = 2, CategoryName = "Thịt bò" },
                new Category { CategoryID = 3, CategoryName = "Thịt heo" },
                new Category { CategoryID = 4, CategoryName = "Cơm & Canh & Mỳ" },
                new Category { CategoryID = 5, CategoryName = "Lẩu" },
                new Category { CategoryID = 6, CategoryName = "Hải sản" },
                new Category { CategoryID = 7, CategoryName = "Tráng miệng" }
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();

            // Seed Products
            var products = new Product[]
            {
                new Product { ProductID = 1, ProductName = "Salad Cá hồi", Price = 89000, Image = "fdd8063e-ba98-4b94-83fa-de143613995b.jpg", CategoryID = 1 },
                new Product { ProductID = 2, ProductName = "Há cảo truyền thống Hàn Quốc", Price = 79000, Image = "c828593b-9428-4410-b8f8-af729b48bc42.jpg", CategoryID = 1 },
                new Product { ProductID = 3, ProductName = "Salad mùa xuân", Price = 79000, Image = "fee9ec44-db9f-44f8-a29f-4a3f7c1b279d.jpg", CategoryID = 1 },
                new Product { ProductID = 4, ProductName = "Gầu bò Mỹ xốt mật ong 200g", Price = 169000, Image = "1e1d6051-f56e-4952-99e0-126f9137dca4.jpg", CategoryID = 2 },
                new Product { ProductID = 5, ProductName = "Sườn bò hảo hạng xốt Obanthan 200g", Price = 449000, Image = "e5892803-3602-4093-a297-e59e6e5ca127.jpg", CategoryID = 2 },
                new Product { ProductID = 6, ProductName = "Sườn non bò Mỹ hảo hạng tươi 100g", Price = 219000, Image = "e699c2db-3e48-4483-9b95-06b5a5e28034.jpg", CategoryID = 2 },
                new Product { ProductID = 7, ProductName = "Má heo Mỹ tươi/sốt obathan", Price = 149000, Image = "8d20e3ff-0bee-4926-961a-62353e549df9.jpg", CategoryID = 3 },
                new Product { ProductID = 8, ProductName = "Sườn heo gabi", Price = 139000, Image = "5aa1697e-824f-475e-91c2-c2d837c69e48.jpg", CategoryID = 3 },
                new Product { ProductID = 9, ProductName = "Nạc vai heo Mỹ sốt OBT/tươi", Price = 109000, Image = "31583f79-182f-4181-899e-82716dcb6be9.jpg", CategoryID = 3 },
                new Product { ProductID = 10, ProductName = "Canh lòng bò ưu đãi", Price = 250000, Image = "83113b0e-cb34-47c9-973b-b6fd758113dd.jpg", CategoryID = 4 },
                new Product { ProductID = 11, ProductName = "Miến xào", Price = 100000, Image = "96bf7404-97c9-4267-91df-a91021f96a81.jpg", CategoryID = 4 },
                new Product { ProductID = 12, ProductName = "Mỳ tương đen", Price = 100000, Image = "e6eadfc8-4e69-454b-9081-39ad9492a045.jpg", CategoryID = 4 },
                new Product { ProductID = 13, ProductName = "Lẩu dê", Price = 389000, Image = "b73e1d0e-5ff6-4868-93de-a4e8650f8648.jpg", CategoryID = 5 },
                new Product { ProductID = 14, ProductName = "Rau lẩu", Price = 35000, Image = "f2b4b3ae-9820-4868-b3b3-fd043d9fd56f.jpg", CategoryID = 5 },
                new Product { ProductID = 15, ProductName = "Lẩu kim chi cỡ lớn", Price = 299000, Image = "874c9518-7901-4765-a09d-85b30a03b83e.jpg", CategoryID = 5 },
                new Product { ProductID = 16, ProductName = "Bào ngư", Price = 309000, Image = "81d99c67-e398-49ad-9b65-13ee4b5accf2.jpg", CategoryID = 6 },
                new Product { ProductID = 17, ProductName = "Tôm nướng Gogi", Price = 229000, Image = "4f702a87-1652-4580-b104-a7bb5b5098ad.jpg", CategoryID = 6 },
                new Product { ProductID = 18, ProductName = "Cá hồi", Price = 179000, Image = "3b91b24c-fdd6-4899-a78c-9ae1cc1efb04.jpg", CategoryID = 6 },
                new Product { ProductID = 19, ProductName = "Kem Caramen Flan cake", Price = 15000, Image = "838b5d02-7cda-4e6d-b597-6137ab067643.jpg", CategoryID = 7 },
                new Product { ProductID = 20, ProductName = "Mochi socola", Price = 15000, Image = "3546ae37-7a43-4175-957a-f090ca2b1568.jpg", CategoryID = 7 },
                new Product { ProductID = 21, ProductName = "Mochi trà xanh", Price = 15000, Image = "bc4ae083-cddb-43a2-86bb-c974cee09ea4.jpg", CategoryID = 7 }
            };

            context.Products.AddRange(products);
            context.SaveChanges();

            // Seed Employees
            var employees = new Employee[]
            {
                new Employee { EmployeeID = "huynguyen", EmployeeName = "Huy Nguyễn", PhoneNumber = "0788859370", Address = "Lấp Vò, Đồng Tháp", Password = "B8DC042D8CF7DEEFB0EC6A264C930B02", Role = false }
            };
            context.Employees.AddRange(employees);
            context.SaveChanges();

            // Seed Tables
            var tables = new Table[]
            {
                new Table { TableID = "101", IsOccupied = false },
                new Table { TableID = "102", IsOccupied = false },
                new Table { TableID = "103", IsOccupied = false },
                new Table { TableID = "104", IsOccupied = false },
                new Table { TableID = "105", IsOccupied = false },
                new Table { TableID = "106", IsOccupied = false },
                new Table { TableID = "107", IsOccupied = false }
            };
            context.Tables.AddRange(tables);
            context.SaveChanges();
        }
    }
}
