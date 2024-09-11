namespace OrderingSystemData.Models
{
    public class Table
    {
        public Table()
        {
            this.Orders = new HashSet<Order>();
        }
        public string TableID { get; set; } 
        public bool IsOccupied { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
