using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystemDTO
{
    public class OrderDTO
    {
        public int OrderID { get; set; }
        public DateTime? OrderDate { get; set; }

        [Required(ErrorMessage = "Trạng thái là bắt buộc")]
        [StringLength(50, ErrorMessage = "Độ dài tối đa của trạng thái là 50 ký tự")]
        public string Status { get; set; }
        [Required(ErrorMessage = "Mã nhân viên là bắt buộc")]
        public string EmployeeID { get; set; }
        public string? EmployeeName { get; set; }
        [Required(ErrorMessage = "Mã bàn ănlà bắt buộc")]
        public string TableID { get; set; }
    }
}
