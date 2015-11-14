using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CasinoRobot.Converters
{
    public class AnyBoolToVisibilityConverter : IMultiValueConverter
    {

        public static AnyBoolToVisibilityConverter Instance = new AnyBoolToVisibilityConverter();

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return values.Any(cur => cur is bool && (bool)cur) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
