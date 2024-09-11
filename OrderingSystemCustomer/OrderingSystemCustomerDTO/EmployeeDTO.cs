using System.ComponentModel.DataAnnotations;

namespace OrderingSystemCustomerDTO
{
    public class EmployeeDTO
    {
        [Required(ErrorMessage = "Mã nhân viên là bắt buộc")]
        [StringLength(50, ErrorMessage = "Độ dài tối đa của mã nhân viên là 50 ký tự")]
        public string EmployeeID { get; set; }

        [Required(ErrorMessage = "Tên nhân viên là bắt buộc")]
        [StringLength(100, ErrorMessage = "Độ dài tối đa của tên nhân viên là 100 ký tự")]
        public string EmployeeName { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [StringLength(15, ErrorMessage = "Độ dài tối đa của số điện thoại là 15 ký tự")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Địa chỉ là bắt buộc")]
        [StringLength(255, ErrorMessage = "Độ dài tối đa của địa chỉ là 255 ký tự")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Vai trò là bắt buộc")]
        public bool Role { get; set; }
    }
}
