using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CasinoRobot.Converters
{
    public class AreEqualConverter : IMultiValueConverter
    {

        public static AreEqualConverter Instance = new AreEqualConverter();

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var validValues = values.Where(cur => cur != null).ToList();
            if (validValues.Count < 2)
                return false;

            return validValues.All(cur => cur == validValues[0]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
