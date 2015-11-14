using CasinoRobot.ViewModels;
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
    public class GetStreakChangeColorConverter : IMultiValueConverter
    {

        public static GetStreakChangeColorConverter Instance = new GetStreakChangeColorConverter();

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Count() != 2)
                throw new ArgumentException();

            int streakCount = (int)values[0];
            int maxStreakCount = (int)values[1];

            if (streakCount == 0)
                return Brushes.White;
            if (streakCount < ApplicationViewModel.Instance.Settings.MinBettingStreakCount)
                return Brushes.LightGreen;
            //if (streakCount > ApplicationViewModel.Instance.Settings.MaxBettingStreakCount)
            //    return Brushes.Red;

            return Brushes.Orange;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
