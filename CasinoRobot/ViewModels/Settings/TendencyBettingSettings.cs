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
    public class TendencyBettingSettings
    {
        public bool ManualBetOnRed { get; set; }
        public bool ManualBetOnBlack { get; set; }
        public bool ManualBetOnEven { get; set; }
        public bool ManualBetOnOdd { get; set; }
        public bool ManualBetOnTo18 { get; set; }
        public bool ManualBetOnFrom19 { get; set; }

    }
}
