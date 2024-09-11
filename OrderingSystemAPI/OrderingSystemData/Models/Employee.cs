namespace OrderingSystemData.Models
{
    public class Employee
    {
        public Employee()
        {
            this.Orders = new HashSet<Order>();
        }
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public bool Role { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
