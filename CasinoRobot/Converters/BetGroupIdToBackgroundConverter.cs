using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace CasinoRobot.Converters
{
    public class BetGroupIdToBackgroundConverter : IValueConverter
    {

        public static BetGroupIdToBackgroundConverter Instance = new BetGroupIdToBackgroundConverter();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int intValue = System.Convert.ToInt32(value);
            if (intValue % 2 == 1)
                return new SolidColorBrush(Color.FromRgb(0x52, 0xB9, 0xFA));

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
