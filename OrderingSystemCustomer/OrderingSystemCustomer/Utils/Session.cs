using OrderingSystemCustomerDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystemCustomer.Utils
{
    public static class Session
    {
        public static string CurrentTableID { get; set; }
        public static EmployeeDTO Employee { get; set; }
        public static int CurrentOrderID { get; set; }
        public static List<OrderDetailDTO> OrderDetails { get; set; }
    }
}
