using CasinoRobot.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CasinoRobot.Converters
{
    public class IsNumberOfTypeConverter : IValueConverter
    {

        public static IsNumberOfTypeConverter Instance = new IsNumberOfTypeConverter();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int number = System.Convert.ToInt32(value);
            string stringParameter = (string)parameter;
            if (number == 0 && stringParameter != "Zero")
                return false;

            if (stringParameter == "Red")
                return RouletteHelper.IsNumberOfType(number, NumberKind.Red);
            if (stringParameter == "Black")
                return RouletteHelper.IsNumberOfType(number, NumberKind.Black);
            if (stringParameter == "Zero")
                return RouletteHelper.IsNumberOfType(number, NumberKind.Zero);
            if (stringParameter == "Odd")
                return RouletteHelper.IsNumberOfType(number, NumberKind.Odd);
            if (stringParameter == "Even")
                return RouletteHelper.IsNumberOfType(number, NumberKind.Even);
            if (stringParameter == "To18")
                return RouletteHelper.IsNumberOfType(number, NumberKind.To18);
            if (stringParameter == "From19")
                return RouletteHelper.IsNumberOfType(number, NumberKind.From19);

                    return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
