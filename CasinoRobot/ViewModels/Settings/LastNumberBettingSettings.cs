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
    public class LastNumberBettingSettings
    {
        public bool BetOnRedAndBlack { get; set; }
        public bool BetOnEvenAndOdd { get; set; }
        public bool BetOnTo18AndFrom19 { get; set; }
        public int AlternateAfterLossCount { get; set; }
        public bool IsTendencyAdjustmentEnabled { get; set; }
        /// <summary>
        /// Count of spins to consider for tendency adjustment
        /// </summary>
        public int TendencyAdjustmentMeasurementCount { get; set; }
        /// <summary>
        /// Count of losses in Measurment Count, to adjust
        /// </summary>
        public int TendencyAdjustmentLossThreshold { get; set; }

        public LastNumberBettingSettings()
        {
            BetOnRedAndBlack = true;
            BetOnEvenAndOdd = true;
            BetOnTo18AndFrom19 = true;
            AlternateAfterLossCount = 0;

            IsTendencyAdjustmentEnabled = false;
            TendencyAdjustmentMeasurementCount = 5;
            TendencyAdjustmentLossThreshold = 4;
        }
    }
}
