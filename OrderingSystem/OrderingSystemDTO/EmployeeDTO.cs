using System.ComponentModel.DataAnnotations;

namespace OrderingSystemDTO
{
    public class EmployeeDTO
    {
        [Required(ErrorMessage = "Mã nhân viên là bắt buộc")]
        [StringLength(50, ErrorMessage = "Độ dài tối đa của mã nhân viên là 50 ký tự")]
        [MinLength(2, ErrorMessage = "Độ dài tối thiểu của mã nhân viên là 2 ký tự")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Mã nhân viên chỉ được nhập chữ và số")]
        public string EmployeeID { get; set; }

        [Required(ErrorMessage = "Tên nhân viên là bắt buộc")]
        [StringLength(100, ErrorMessage = "Độ dài tối đa của tên nhân viên là 100 ký tự")]
        [MinLength(2, ErrorMessage = "Độ dài tối thiểu của tên nhân viên là 2 ký tự")]
        public string EmployeeName { get; set; }

        [RegularExpression(@"^(?:\+?84|0)(\d{9,10})$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Địa chỉ là bắt buộc")]
        [StringLength(255, ErrorMessage = "Độ dài tối đa của địa chỉ là 255 ký tự")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Vai trò là bắt buộc")]
        public bool Role { get; set; }
    }
}
