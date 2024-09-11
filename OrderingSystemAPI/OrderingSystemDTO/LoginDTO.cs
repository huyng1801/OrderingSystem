using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystemDTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Mã nhân viên là bắt buộc")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Mã nhân viên chỉ được nhập chữ và số")]
        public string EmployeeID { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        public string Password { get; set; }
    }

}
