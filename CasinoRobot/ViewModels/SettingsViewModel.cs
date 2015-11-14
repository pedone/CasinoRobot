using CasinoRobot.Betting;
using CasinoRobot.Enums;
using CasinoRobot.Helpers;
using CasinoRobot.ViewModels.Settings;
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

namespace CasinoRobot.ViewModels
{

    public class SettingsViewModel : ViewModel
    {
        public string DefaultDirectory { get; private set; }

        private List<CasinoBetButtonViewModel> _CasinoBets;
        public ReadOnlyCollection<CasinoBetButtonViewModel> CasinoBets { get; private set; }
        private List<CasinoButtonViewModel> _CasinoButtons;
        public ReadOnlyCollection<CasinoButtonViewModel> CasinoButtons { get; private set; }

        private List<CasinoNumberViewModel> _CasinoNumbers;
        public ReadOnlyCollection<CasinoNumberViewModel> CasinoNumbers { get; private set; }
        public SolidColorBrush TransparentBackground { get; set; }
        public SolidColorBrush DefaultBackground { get; set; }
        public System.Windows.Point CasinoAreaWindowOffset { get; set; }
        public System.Windows.Point FixedCasinoWindowPosition { get; set; }
        public System.Windows.Point FixedWindowPosition { get; set; }
        public System.Drawing.Bitmap CasinoScreenshot { get; set; }

        public LastNumberBettingSettings LastNumberBettingSettings { get; private set; }
        public SingleStreakBettingSettings SingleStreakBettingSettings { get; private set; }
        public TendencyBettingSettings TendencyBettingSettings { get; private set; }
        public NumberNegligenceBettingSettings NumberNegligenceBettingSettings { get; private set; }

        public System.Windows.Size CasinoAreaSize { get; set; }

        private BettingSystem _BettingMode = BettingSystem.Martingale;
        public BettingSystem BettingMode
        {
            get { return _BettingMode; }
            set
            {
                _BettingMode = value;
                ApplicationViewModel.Instance.BetManager.UpdateBettingModeInstance(_BettingMode);

                FirePropertyChanged("BettingMode");
            }
        }

        private double _MaxBetAmount = 1500;
        public double MaxBetAmount
        {
            get { return _MaxBetAmount; }
            set
            {
                _MaxBetAmount = value;
                FirePropertyChanged("MaxBetAmount");
            }
        }

        private double _MinBetAmount = 1;
        public double MinBetAmount
        {
            get { return _MinBetAmount; }
            set
            {
                _MinBetAmount = value;
                FirePropertyChanged("MinBetAmount");
            }
        }

        private double _StartBalance = 100;
        public double StartBalance
        {
            get { return _StartBalance; }
            set
            {
                _StartBalance = value;
                FirePropertyChanged("StartBalance");
            }
        }

        private SpeedKind _SessionSpeed = SpeedKind.Fast;
        public SpeedKind SessionSpeed
        {
            get { return _SessionSpeed; }
            set
            {
                _SessionSpeed = value;
                FirePropertyChanged("SessionSpeed");
            }
        }

        private string _Logfile;
        public string Logfile
        {
            get { return _Logfile; }
            set
            {
                _Logfile = value;
                FirePropertyChanged("Logfile");
            }
        }

        private int _AutoMaxStreakDecrease = 8;
        public int AutoMaxStreakDecrease
        {
            get { return _AutoMaxStreakDecrease; }
            set
            {
                _AutoMaxStreakDecrease = value;
                FirePropertyChanged("AutoMaxStreakDecrease");
            }
        }

        private int _AutoBetStreakThresholdDecrease = 5;
        public int AutoBetStreakThresholdDecrease
        {
            get { return _AutoBetStreakThresholdDecrease; }
            set
            {
                _AutoBetStreakThresholdDecrease = value;
                FirePropertyChanged("AutoBetStreakThresholdDecrease");
            }
        }

        private int _TimeLimit = 0;
        public int TimeLimit
        {
            get { return _TimeLimit; }
            set
            {
                _TimeLimit = value;
                FirePropertyChanged("TimeLimit");
            }
        }

        private int _MaxLostAmount = 0;
        public int MaxLostAmount
        {
            get { return _MaxLostAmount; }
            set
            {
                _MaxLostAmount = value;
                FirePropertyChanged("MaxLostAmount");
            }
        }

        private int _MaxWonAmount = 200;
        public int MaxWonAmount
        {
            get { return _MaxWonAmount; }
            set
            {
                _MaxWonAmount = value;
                FirePropertyChanged("MaxWonAmount");
            }
        }

        private int _MaxLossCount = 0;
        public int MaxLossCount
        {
            get { return _MaxLossCount; }
            set
            {
                _MaxLossCount = value;
                FirePropertyChanged("MaxLossCount");
            }
        }

        private int _MaxWinCount = 0;
        public int MaxWinCount
        {
            get { return _MaxWinCount; }
            set
            {
                _MaxWinCount = value;
                FirePropertyChanged("MaxWinCount");
            }
        }

        private int _TimeToRefresh = 3;
        /// <summary>
        /// Time in minutes, after which to refresh the browser.
        /// </summary>
        public int TimeToRefresh
        {
            get { return _TimeToRefresh; }
            set
            {
                _TimeToRefresh = value;
                FirePropertyChanged("TimeToRefresh");
            }
        }

        private bool _ResetStreaksOnZero;
        public bool ResetStreaksOnZero
        {
            get { return _ResetStreaksOnZero; }
            set
            {
                _ResetStreaksOnZero = value;
                FirePropertyChanged("ResetStreaksOnZero");
            }
        }

        private bool _IsSimulationMode = true;
        public bool IsSimulationMode
        {
            get { return _IsSimulationMode; }
            set
            {
                _IsSimulationMode = value;
                FirePropertyChanged("IsSimulationMode");
            }
        }

        private bool _DoubleUpOnLoss = true;
        public bool DoubleUpOnLoss
        {
            get { return _DoubleUpOnLoss; }
            set
            {
                _DoubleUpOnLoss = value;
                FirePropertyChanged("DoubleUpOnLoss");
            }
        }

        private bool _EnableMultiStreakBets = false;
        public bool EnableMultiStreakBets
        {
            get { return _EnableMultiStreakBets; }
            set
            {
                _EnableMultiStreakBets = value;
                FirePropertyChanged("EnableMultiStreakBets");
            }
        }

        private int _MultiStreakBetStartBalance = 300;
        public int MultiStreakBetStartBalance
        {
            get { return _MultiStreakBetStartBalance; }
            set
            {
                _MultiStreakBetStartBalance = value;
                FirePropertyChanged("MultiStreakBetStartBalance");
            }
        }

        /// <summary>
        /// Maximum number of bets per betting streak.
        /// </summary>
        public int MaxBettingStreakLength
        {
            get { return MaxBettingStreakCount - MinBettingStreakCount + 1; }
        }

        private bool _AutoAdjustBettingStreakCount = true;
        public bool AutoAdjustBettingStreakCount
        {
            get { return _AutoAdjustBettingStreakCount; }
            set
            {
                _AutoAdjustBettingStreakCount = value;
                FirePropertyChanged("AutoAdjustBettingStreakCount");
            }
        }

        private int _MaxBettingStreakCount = 12;
        public int MaxBettingStreakCount
        {
            get { return _MaxBettingStreakCount; }
            set
            {
                _MaxBettingStreakCount = value;
                FirePropertyChanged("MaxBettingStreakCount");
            }
        }

        private int _MinBettingStreakCount = 8;
        public int MinBettingStreakCount
        {
            get { return _MinBettingStreakCount; }
            set
            {
                _MinBettingStreakCount = value;
                FirePropertyChanged("MinBettingStreakCount");
            }
        }

        private CasinoBetButtonViewModel _DefaultBet;
        public CasinoBetButtonViewModel DefaultBet
        {
            get { return _DefaultBet; }
            set
            {
                _DefaultBet = value;
                FirePropertyChanged("DefaultBet");
            }
        }

        private int _WindowHeight = 600;
        public int WindowHeight
        {
            get { return _WindowHeight; }
            set
            {
                _WindowHeight = value;
                ApplicationViewModel.MainWindow.Height = _WindowHeight;

                FirePropertyChanged("WindowHeight");
            }
        }

        private bool _IsBettingEnabled = true;
        public bool IsBettingEnabled
        {
            get { return _IsBettingEnabled; }
            set
            {
                _IsBettingEnabled = value;
                FirePropertyChanged("IsBettingEnabled");
            }
        }

        private int _WindowWidth = 1024;
        public int WindowWidth
        {
            get { return _WindowWidth; }
            set
            {
                _WindowWidth = value;
                ApplicationViewModel.MainWindow.Width = _WindowWidth;

                FirePropertyChanged("WindowWidth");
            }
        }

        public SettingsViewModel()
        {
            LastNumberBettingSettings = new LastNumberBettingSettings();
            SingleStreakBettingSettings = new SingleStreakBettingSettings();
            TendencyBettingSettings = new TendencyBettingSettings();
            NumberNegligenceBettingSettings = new NumberNegligenceBettingSettings();

            DefaultDirectory = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "CasinoRobot");
            Logfile = Path.Combine(DefaultDirectory, "log.txt");
            DefaultBackground = new SolidColorBrush(Colors.White);
            TransparentBackground = new SolidColorBrush(Colors.White)
            {
                Opacity = 0
            };

            InitCasinoButtons();
        }

        private void InitCasinoButtons()
        {
            //Buttons
            _CasinoButtons = new List<CasinoButtonViewModel>()
            {
                new CasinoButtonViewModel { Kind = RouletteButtonKind.Turn, Name = "Turn"},
                new CasinoButtonViewModel { Kind = RouletteButtonKind.RemoveBets, Name = "Remove Bets"},
                new CasinoButtonViewModel { Kind = RouletteButtonKind.Black, Name = "Black"},
                new CasinoButtonViewModel { Kind = RouletteButtonKind.Red, Name = "Red"},
                new CasinoButtonViewModel { Kind = RouletteButtonKind.Odd, Name = "Odd"},
                new CasinoButtonViewModel { Kind = RouletteButtonKind.Even, Name = "Even"},
                new CasinoButtonViewModel { Kind = RouletteButtonKind.To18, Name = "1 To 18"},
                new CasinoButtonViewModel { Kind = RouletteButtonKind.From19, Name = "19 To 36"},
                new CasinoButtonViewModel { Kind = RouletteButtonKind.RefreshPosition, Name = "Refresh Position", IsPositionAbsolute = true},
            };
            CasinoButtons = new ReadOnlyCollection<CasinoButtonViewModel>(_CasinoButtons);

            //Numbers
            _CasinoNumbers = new List<CasinoNumberViewModel>();
            CasinoNumbers = new ReadOnlyCollection<CasinoNumberViewModel>(_CasinoNumbers);
            for (int i = 0; i <= 36; i++)
                _CasinoNumbers.Add(new CasinoNumberViewModel { Number = i });

            //Bets
            _CasinoBets = new List<CasinoBetButtonViewModel>()
            {
                new CasinoBetButtonViewModel { Amount = 0.1, MultiBetStartBalance = 0},
                new CasinoBetButtonViewModel { Amount = 0.5, MultiBetStartBalance = 100 },
                new CasinoBetButtonViewModel { Amount = 1, MultiBetStartBalance = 200, IsAvailable = true },
                new CasinoBetButtonViewModel { Amount = 5, MultiBetStartBalance = 400, IsAvailable = true  },
                new CasinoBetButtonViewModel { Amount = 25, MultiBetStartBalance = 1000, IsAvailable = true  },
                new CasinoBetButtonViewModel { Amount = 100, MultiBetStartBalance = 4000 },
            };
            CasinoBets = new ReadOnlyCollection<CasinoBetButtonViewModel>(_CasinoBets);

            DefaultBet = CasinoBets.FirstOrDefault(cur => cur.Amount == 1);
        }

        public void Save(Stream stream)
        {
            //TODO update

            var settingsElements = new XElement("Settings",
                new XAttribute("WindowWidth", WindowWidth),
                new XAttribute("WindowHeight", WindowHeight),
                new XElement("WindowPosition",
                    new XAttribute("X", FixedWindowPosition.X),
                    new XAttribute("Y", FixedWindowPosition.Y)),
                new XElement("ButtonMarkers",
                    from marker in CasinoButtons
                    where marker.HasValue
                    select new XElement("Marker",
                        new XAttribute("ButtonKind", marker.Kind),
                        new XAttribute("X", marker.Position.Value.X),
                        new XAttribute("Y", marker.Position.Value.Y))),
            new XElement("NumberMarkers",
                from marker in CasinoNumbers
                where marker.HasValue
                select new XElement("Marker",
                   new XAttribute("Number", marker.Number),
                   from point in marker.Area
                   select new XElement("Point",
                       new XAttribute("X", point.X),
                       new XAttribute("Y", point.Y)))),
            new XElement("Bets",
                from bet in CasinoBets
                where bet.HasValue
                select new XElement("Bet",
                    new XAttribute("Amount", bet.Amount),
                    new XAttribute("X", bet.Position.Value.X),
                    new XAttribute("Y", bet.Position.Value.Y))));

            settingsElements.Save(stream);
        }

        public void Load(Stream stream)
        {
            //TODO update

            XElement settingsElement = XElement.Load(stream);
            var windowWidth = Convert.ToInt32(settingsElement.Attribute("WindowWidth").Value);
            var windowHeight = Convert.ToInt32(settingsElement.Attribute("WindowHeight").Value);
            WindowWidth = windowWidth;
            WindowHeight = windowHeight;

            XElement windowPositionElement = settingsElement.Element("WindowPosition");
            var windowPosX = Convert.ToDouble(windowPositionElement.Attribute("X").Value);
            var windowPosY = Convert.ToDouble(windowPositionElement.Attribute("Y").Value);
            FixedWindowPosition = new Point(windowPosX, windowPosY);
            ApplicationViewModel.MainWindow.Left = windowPosX;
            ApplicationViewModel.MainWindow.Top = windowPosY;

            XElement buttonMarkersElement = settingsElement.Element("ButtonMarkers");
            XElement numberMarkersElement = settingsElement.Element("NumberMarkers");
            XElement betsElement = settingsElement.Element("Bets");

            bool hasButtonMarkers = buttonMarkersElement != null && buttonMarkersElement.Elements("Marker").Count() > 0;
            bool hasNumberMarkers = numberMarkersElement != null && numberMarkersElement.Elements("Marker").Count() > 0;
            bool hasBets = betsElement != null && betsElement.Elements("Bet").Count() > 0;

            if (hasButtonMarkers)
                foreach (var element in buttonMarkersElement.Elements("Marker"))
                {
                    var buttonKind = (RouletteButtonKind)Enum.Parse(typeof(RouletteButtonKind), element.Attribute("ButtonKind").Value);
                    var posX = Convert.ToDouble(element.Attribute("X").Value, CultureInfo.InvariantCulture);
                    var posY = Convert.ToDouble(element.Attribute("Y").Value, CultureInfo.InvariantCulture);

                    var markerViewModel = CasinoButtons.First(cur => cur.Kind == buttonKind);
                    markerViewModel.Position = new Point(posX, posY);
                }

            if (hasNumberMarkers)
                foreach (var element in numberMarkersElement.Elements("Marker"))
                {
                    var number = Convert.ToInt32(element.Attribute("Number").Value);
                    var markerViewModel = CasinoNumbers.First(cur => cur.Number == number);
                    markerViewModel.Area.Clear();

                    foreach (var point in element.Elements("Point"))
                    {
                        var curPoint = new Point(Convert.ToDouble(point.Attribute("X").Value, CultureInfo.InvariantCulture),
                                         Convert.ToDouble(point.Attribute("Y").Value, CultureInfo.InvariantCulture));
                        markerViewModel.Area.Add(curPoint);
                    }
                }

            if (hasBets)
                foreach (var element in betsElement.Elements("Bet"))
                {
                    var amount = Convert.ToDouble(element.Attribute("Amount").Value, CultureInfo.InvariantCulture);
                    var posX = Convert.ToDouble(element.Attribute("X").Value, CultureInfo.InvariantCulture);
                    var posY = Convert.ToDouble(element.Attribute("Y").Value, CultureInfo.InvariantCulture);

                    var betViewModel = CasinoBets.First(cur => cur.Amount == amount);
                    betViewModel.Position = new Point(posX, posY);
                }
        }

        public CasinoButtonViewModel GetButton(RouletteButtonKind buttonKind)
        {
            return CasinoButtons.First(cur => cur.Kind == buttonKind);
        }


        internal string ValidateSettings()
        {
            if (CasinoButtons.Any(cur => !cur.HasValue))
                return "All Casino Buttons must be assigned!";
            if (CasinoNumbers.Any(cur => !cur.HasValue))
                return "All Casino Numbers must be assigned!";
            if (CasinoBets.All(cur => !cur.HasValue))
                return "At least one Bet must be assigned!";
            if (CasinoScreenshot == null)
                return "The casino screenshot must be taken to start!";
            if (!DefaultBet.HasValue)
                return "Default bet must have a value to start!";
            if (DefaultBet.Amount < MinBetAmount)
                return "Default bet is less than min bet amount!";
            if (MinBettingStreakCount > MaxBettingStreakCount)
                return "Min Betting Streak must be less than or equal to Max Betting Streak";

            return string.Empty;
        }
    }
}
