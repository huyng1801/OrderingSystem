using System.ComponentModel.DataAnnotations;

namespace OrderingSystemDTO
{
    public class CategoryDTO
    {
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Tên danh mục là bắt buộc")]
        [StringLength(50, ErrorMessage = "Độ dài tối đa của tên danh mục là 50 ký tự")]
        [MinLength(2, ErrorMessage = "Độ dài tối thiểu của tên danh mục là 2 ký tự")]
        public string CategoryName { get; set; }

    }
}
