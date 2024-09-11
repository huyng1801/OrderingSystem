using Microsoft.EntityFrameworkCore;

namespace OrderingSystemData.Models
{
    public class OrderingSystemContext : DbContext
    {
        public OrderingSystemContext()
        {
        }
        public OrderingSystemContext(DbContextOptions<OrderingSystemContext> options) : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryID)
                    .HasName("PK_CategoryID");
                entity.HasIndex(e => e.CategoryName, "UQ_CategoryName")
                .IsUnique();
                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(50); 
            });
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductID)
                    .HasName("PK_ProductID");
                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(100); 

                entity.Property(e => e.Description)
                    .HasMaxLength(255);

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasMaxLength(255); 

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .IsRequired()
                    .HasForeignKey(d => d.CategoryID)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Product_Category");
            });
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeID)
                    .HasName("PK_EmployeeID");
                entity.Property(e => e.EmployeeID)
                    .IsRequired()
                    .HasMaxLength(50); 

                entity.Property(e => e.EmployeeName)
                    .IsRequired()
                    .HasMaxLength(100); 

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(15); 

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(255); 

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(128); 
            });
            modelBuilder.Entity<Table>(entity =>
            {
                entity.HasKey(e => e.TableID)
                    .HasName("PK_TableEatID");
                entity.Property(e => e.TableID)
                    .HasMaxLength(10);
                entity.Property(e => e.IsOccupied)
                    .IsRequired();

            });
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderID)
                    .HasName("PK_OrderID");
                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50); 
                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.EmployeeID)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Order_Employee");
                entity.HasOne(d => d.Table)
                    .WithMany(p => p.Orders)
                    .IsRequired()
                    .HasForeignKey(d => d.TableID)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Order_Table");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => e.OrderDetailID)
                  .HasName("PK_OrderDetailID");
                entity.Property(e => e.UnitePrice)
                .IsRequired();
                entity.Property(e => e.Quantity)
                .IsRequired();
                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .IsRequired()
                    .HasForeignKey(d => d.OrderID)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_OrderDetail_Order");
                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .IsRequired()
                    .HasForeignKey(d => d.ProductID)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_OrderDetail_Product");
            });
        }
    }
}