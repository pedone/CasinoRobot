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
    public class PointsToPointCollectionConverter : IValueConverter
    {

        public static PointsToPointCollectionConverter Instance = new PointsToPointCollectionConverter();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IEnumerable<Point> points = value as IEnumerable<Point>;
            if (points == null || points.Count() == 0)
                return null;

            return new PointCollection(points);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
