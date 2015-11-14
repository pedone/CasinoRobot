using CasinoRobot.Betting;
using CasinoRobot.Helpers;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace CasinoRobot.ViewModels.Settings
{
    public class SingleStreakBettingSettings : ViewModel
    {
        public bool BetOnRedAndBlack { get; set; }
        public bool BetOnEvenAndOdd { get; set; }
        public bool BetOnTo18AndFrom19 { get; set; }

        public List<int> StreakCounts { get; set; }
        public SingleStreakBettingSettings()
        {
            BetOnRedAndBlack = true;
            BetOnEvenAndOdd = false;
            BetOnTo18AndFrom19 = false;
            StreakCounts = new List<int> { 1, 2, 3 };
        }
    }
}
