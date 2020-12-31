using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace AdvancedSerialCommunicator.Converters
{
    public class BoolToColourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool val)
            {
                return val ? new SolidColorBrush(Color.FromRgb(40, 200, 40)) : new SolidColorBrush(Color.FromRgb(200, 40, 40));
                //if (parameter is string twos)
                //{
                //    string[] cases = twos.Split('#');
                //    if (cases.Length > 1)
                //    {
                //        if (val)
                //        {
                //            SolidColorBrush c1 = ColorConverter.ConvertFromString(cases[0]) as SolidColorBrush;
                //            if (c1 != null)
                //            {
                //                return c1;
                //
                //            }
                //        }
                //        else
                //        {
                //            SolidColorBrush c2 = ColorConverter.ConvertFromString(cases[1]) as SolidColorBrush;
                //            if (c2 != null)
                //            {
                //                return c2;
                //
                //            }
                //        }
                //    }
                //}
            }

            return new SolidColorBrush(Colors.Red);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
