using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AdvancedSerialCommunicator.Converters
{
    public class BoolToCustomStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isTrue)
            {
                if (parameter is string twos)
                {
                    string[] cases = twos.Split('#');
                    if (cases.Length > 1)
                    {
                        return isTrue ? cases[0] : cases[1];
                    }
                }
            }
            return "Error (BTIC_C)";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }
}
