using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CasinoRobot.Converters
{
    public class SpinsToDropConverter : IValueConverter
    {

        public static SpinsToDropConverter Instance = new SpinsToDropConverter();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                var intValue = System.Convert.ToInt32(value);
                return 36 - intValue % 36;
            }
            catch
            {
                return "Error";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
