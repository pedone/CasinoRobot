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
    public class NumberNegligenceBettingSettings
    {
        public int MinNumberBettingCount { get; set; }
        public int MaxNumberBettingCount { get; set; }
        public int NegligenceStreakThreshold { get; set; }
        public int WaitForRepetitionCount { get; set; }
        public int AbortWaitOnLossCount { get; set; }
        public bool MaxStreakCountNumberLast { get; set; }
        public bool FrequentNumbersFirst { get; set; }
        public bool BetOnSameEndings { get; set; }

        public NumberNegligenceBettingSettings()
        {
            MinNumberBettingCount = 1;
            MaxNumberBettingCount = 4;
            NegligenceStreakThreshold = 60;
            AbortWaitOnLossCount = 6;

            MaxStreakCountNumberLast = false;
            FrequentNumbersFirst = true;
            BetOnSameEndings = true;
            WaitForRepetitionCount = 2;
        }
    }
}
