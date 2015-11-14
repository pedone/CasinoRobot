using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CasinoRobot.Converters
{
    public class IsOfTypeConverter : IValueConverter
    {

        public static IsOfTypeConverter Instance = new IsOfTypeConverter();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Type parameterType = parameter as Type;
            if (parameterType != null && value != null)
                return value.GetType() == parameterType;

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
