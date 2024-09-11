using OrderingSystemCustomer.Utils;
using OrderingSystemCustomerDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystemCustomer.Converters
{
    public class TableStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is TableDTO table)
            {
                if (Session.CurrentTableID == null)
                {
                    return table.IsOccupied ? CustomColor.Red : CustomColor.Green;
                }
                else if (table.TableID == Session.CurrentTableID && table.IsOccupied)
                {
                    return CustomColor.Orange;
                }
                else
                {
                    return table.IsOccupied ? CustomColor.Red : CustomColor.Green;
                }
            }
            else
            {
                return CustomColor.Green;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
