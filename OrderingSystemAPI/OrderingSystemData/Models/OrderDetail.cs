namespace OrderingSystemData.Models
{
    public class OrderDetail
    {
        public int OrderDetailID { get; set; }
        public long UnitePrice { get; set; }
        public int Quantity { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public bool IsServed { get; set; }
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
