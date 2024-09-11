namespace OrderingSystemData.Models
{
    public class Category
    {
        public Category()
        {
            this.Products = new HashSet<Product>();
        }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
     
        public virtual ICollection<Product> Products { get; set; }
    }
}
