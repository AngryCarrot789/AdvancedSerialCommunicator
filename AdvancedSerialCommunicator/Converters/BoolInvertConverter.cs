using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AdvancedSerialCommunicator.Converters
{
    public class BoolInvertConverter : IValueConverter
    {
        public bool GetBool(object value)
        {
            if (value is bool b)
            {
                return b;
            }
            return false;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !GetBool(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !GetBool(value);
        }
    }
}
