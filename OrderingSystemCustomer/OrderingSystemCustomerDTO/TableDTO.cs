using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystemCustomerDTO
{
    public class TableDTO
    {

        [Required(ErrorMessage = "Mã bàn là bắt buộc.")]
        public string TableID { get; set; }
        public bool IsOccupied { get; set; }


    }
}
