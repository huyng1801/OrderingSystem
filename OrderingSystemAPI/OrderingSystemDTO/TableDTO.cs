using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystemDTO
{
    public class TableDTO
    {
        [Required(ErrorMessage = "Mã bàn là bắt buộc.")]
        public string TableID { get; set; }
        [Required(ErrorMessage = "Trạng thái bàn là bắt buộc.")]
        public bool IsOccupied { get; set; }
    }
}
