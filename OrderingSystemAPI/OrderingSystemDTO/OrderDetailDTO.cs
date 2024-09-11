using System.ComponentModel.DataAnnotations;

namespace OrderingSystemDTO
{
    public class OrderDetailDTO
    {
        public int OrderDetailID { get; set; }

        [Required(ErrorMessage = "Đơn giá là bắt buộc")]
        [Range(0, long.MaxValue, ErrorMessage = "Giá không được âm.")]
        public long UnitePrice { get; set; }

        [Required(ErrorMessage = "Số lượng là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Mã đơn hàng là bắt buộc")]
        public int OrderID { get; set; }
        [Required(ErrorMessage = "Mã sản phẩm là bắt buộc")]
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public bool IsServed { get; set; }
    }
}
