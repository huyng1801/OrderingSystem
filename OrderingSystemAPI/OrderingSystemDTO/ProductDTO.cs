
using System.ComponentModel.DataAnnotations;

namespace OrderingSystemDTO
{
    public class ProductDTO
    {
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Tên sản phẩm phải có ít nhất 2 ký tự và không quá 100 ký tự.")]
        public string ProductName { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Giá là bắt buộc.")]
        [Range(0, long.MaxValue, ErrorMessage = "Giá không được âm.")]
        public long Price { get; set; }
        public string? Image { get; set; }
        [Required(ErrorMessage = "Mã danh mục là bắt buộc")]
        public int CategoryID { get; set; }
        public string? CategoryName { get; set; }
    }
}
