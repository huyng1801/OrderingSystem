namespace OrderingSystemData.Models
{
    public class Product
    {
        public Product()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
        }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string? Description { get; set; }
        public long Price { get; set; }
        public string Image { get; set; }
        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
