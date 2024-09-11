namespace OrderingSystemData.Models
{
    public class Order
    {
        public Order()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
        }
        public int OrderID { get; set; }
        public DateTime  OrderDate { get; set; }
        public string Status { get; set; }
        public string? EmployeeID { get; set; }
        public string TableID { get; set; }
        public virtual Employee? Employee { get; set; }
        public virtual Table Table { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
