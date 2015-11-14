using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CasinoRobot.Converters
{
    public class IntListToStringConverter : IValueConverter
    {
        public static IntListToStringConverter Instance = new IntListToStringConverter();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string result = string.Empty;

            IEnumerable<int> intList = value as IEnumerable<int>;
            if (intList == null)
                return result;

            foreach (var number in intList)
                result += string.Format(", {0}", number);

            return result.TrimStart(',', ' ');
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string intListAsString = value as string;
            if (string.IsNullOrEmpty(intListAsString))
                return null;

            var numbers = intListAsString.Split(',');
            List<int> intList = new List<int>();
            foreach (var num in numbers)
            {
                int numAsInt = System.Convert.ToInt32(num.Trim());
                intList.Add(numAsInt);
            }

            return intList;
        }
    }
}
