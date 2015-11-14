using CasinoRobot.Betting;
using CasinoRobot.Helpers;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;
using WindowsInput;

namespace CasinoRobot.ViewModels
{
    public class ApplicationViewModel : ViewModel
    {
        private bool _IsEditingMarkers;
        private System.Windows.Shapes.Polyline _To18From19BalancesLine;
        private System.Windows.Shapes.Polyline _EvenOddBalancesLine;
        private System.Windows.Shapes.Polyline _RedBlackBalancesLine;

        public System.Windows.Shapes.Polyline RedBlackBalancesLine
        {
            get
            {
                return _RedBlackBalancesLine;
            }
            private set
            {
                _RedBlackBalancesLine = value;
                FirePropertyChanged("RedBlackBalancesLine");
            }
        }
        public System.Windows.Shapes.Polyline EvenOddBalancesLine
        {
            get
            {
                return _EvenOddBalancesLine;
            }
            private set
            {
                _EvenOddBalancesLine = value;
                FirePropertyChanged("EvenOddBalancesLine");
            }
        }
        public System.Windows.Shapes.Polyline To18From19BalancesLine
        {
            get
            {
                return _To18From19BalancesLine;
            }
            private set
            {
                _To18From19BalancesLine = value;
                FirePropertyChanged("To18From19BalancesLine");
            }
        }

        public static ApplicationViewModel Instance = new ApplicationViewModel();
        public static MainWindow MainWindow { get; private set; }

        public Point? LastCanvasTranslationPosition { get; set; }
        public static InputSimulator InputSimulator { get; private set; }

        public BettingManager BetManager { get; private set; }
        public SettingsViewModel Settings { get; private set; }
        public StatusViewModel Status { get; private set; }
        public StatisticsViewModel Statistics { get; private set; }

        private ViewModel _CurrentlyAssigningMarker;
        public ViewModel CurrentlyAssigningMarker
        {
            get { return _CurrentlyAssigningMarker; }
            set
            {
                _CurrentlyAssigningMarker = value;
                FirePropertyChanged("CurrentlyAssigningMarker");
            }
        }

        private SessionViewModel _Session;
        public SessionViewModel Session
        {
            get { return _Session; }
            set
            {
                _Session = value;
                FirePropertyChanged("Session");
            }
        }

        private CasinoNumberViewModel _SelectedNumberMarker;
        public CasinoNumberViewModel SelectedNumberMarker
        {
            get { return _SelectedNumberMarker; }
            set
            {
                _SelectedNumberMarker = value;
                FirePropertyChanged("SelectedNumberMarker");
            }
        }

        private CasinoBetButtonViewModel _SelectedBetMarker;
        public CasinoBetButtonViewModel SelectedBetMarker
        {
            get { return _SelectedBetMarker; }
            set
            {
                _SelectedBetMarker = value;
                FirePropertyChanged("SelectedBetMarker");
            }
        }

        private CasinoButtonViewModel _SelectedButtonMarker;
        public CasinoButtonViewModel SelectedButtonMarker
        {
            get { return _SelectedButtonMarker; }
            set
            {
                _SelectedButtonMarker = value;
                FirePropertyChanged("SelectedButtonMarker");
            }
        }

        public bool IsEditingMarkers
        {
            get
            {
                return _IsEditingMarkers;
            }
            set
            {
                _IsEditingMarkers = value;
                if (!_IsEditingMarkers)
                    ResetCanvasPanAndZoom();

                FirePropertyChanged("IsEditingMarkers");
            }
        }

        private bool _ShowSettings;
        public bool ShowSettings
        {
            get { return _ShowSettings; }
            set
            {
                _ShowSettings = value;
                FirePropertyChanged("ShowSettings");
            }
        }

        private bool _ShowMarkers;
        public bool ShowMarkers
        {
            get { return _ShowMarkers; }
            set
            {
                _ShowMarkers = value;
                FirePropertyChanged("ShowMarkers");
            }
        }

        private bool _ShowNumberKindBalances;
        public bool ShowNumberKindBalances
        {
            get { return _ShowNumberKindBalances; }
            set
            {
                _ShowNumberKindBalances = value;
                FirePropertyChanged("ShowNumberKindBalances");
            }
        }

        private bool _ShowWinningsStatisticsLine;
        public bool ShowWinningsStatisticsLine
        {
            get { return _ShowWinningsStatisticsLine; }
            set
            {
                _ShowWinningsStatisticsLine = value;

                if (_ShowWinningsStatisticsLine)
                    Statistics.StatisticsLines.WinningsStatisticsLine.Visibility = Visibility.Visible;
                else
                    Statistics.StatisticsLines.WinningsStatisticsLine.Visibility = Visibility.Collapsed;

                FirePropertyChanged("ShowWinningsStatisticsLine");
            }
        }

        private bool _ShowNumberRepetitionLines;
        public bool ShowNumberRepetitionLines
        {
            get { return _ShowNumberRepetitionLines; }
            set
            {
                _ShowNumberRepetitionLines = value;
                if (_ShowNumberRepetitionLines)
                {
                    Statistics.StatisticsLines.NumberRepetitionsLine.Visibility = Visibility.Visible;
                    Statistics.StatisticsLines.NumberRepetitions2Line.Visibility = Visibility.Visible;
                }
                else
                {
                    Statistics.StatisticsLines.NumberRepetitionsLine.Visibility = Visibility.Collapsed;
                    Statistics.StatisticsLines.NumberRepetitions2Line.Visibility = Visibility.Collapsed;
                }

                FirePropertyChanged("ShowNumberRepetitionLines");
            }
        }

        private bool _IsAdjustingCasinoArea;
        public bool IsAdjustingCasinoArea
        {
            get { return _IsAdjustingCasinoArea; }
            set
            {
                _IsAdjustingCasinoArea = value;
                if (_IsAdjustingCasinoArea)
                    AdjustCasinoAreaBackground(true);
                else
                {
                    UpdateCasinoAreaData();
                    UpdateCurrentCasinoAreaBackground();
                }

                FirePropertyChanged("IsAdjustingCasinoArea");
            }
        }

        private void ResetCanvasPanAndZoom()
        {
            var transform = (MatrixTransform)MainWindow.CasinoCanvas.RenderTransform;
            transform.Matrix = Matrix.Identity;
        }
        static ApplicationViewModel()
        {
            MainWindow = App.Current.MainWindow as MainWindow;
            InputSimulator = new InputSimulator();
        }

        public ApplicationViewModel()
        {
            BetManager = new BettingManager();
            Settings = new SettingsViewModel();
            Session = new SessionViewModel();
            Status = new StatusViewModel();
            Statistics = new StatisticsViewModel();

            RedBlackBalancesLine = new System.Windows.Shapes.Polyline
            {
                StrokeThickness = 2,
                Stroke = Brushes.Red
            };
            EvenOddBalancesLine = new System.Windows.Shapes.Polyline
            {
                StrokeThickness = 2,
                Stroke = Brushes.Black
            };
            To18From19BalancesLine = new System.Windows.Shapes.Polyline
            {
                StrokeThickness = 2,
                Stroke = Brushes.Green
            };
        }

        private void UpdateCasinoAreaData()
        {
            Settings.FixedWindowPosition = new Point(MainWindow.Left, MainWindow.Top);
            Settings.FixedCasinoWindowPosition = GetCurrentCasinoArea().Location;
            Settings.CasinoAreaSize = new System.Windows.Size(MainWindow.CasinoCanvas.ActualWidth, MainWindow.CasinoCanvas.ActualHeight);
        }

        private void UpdateCurrentCasinoAreaBackground()
        {
            Settings.CasinoScreenshot = ScreenHelper.TakeScreenshot(GetCurrentCasinoArea());

            MainWindow.Background = Settings.DefaultBackground;
            MainWindow.CasinoCanvas.Background = new ImageBrush(ScreenHelper.BitmapToImageSource(Settings.CasinoScreenshot));
        }

        public void Init()
        {
            Settings.CasinoAreaWindowOffset = MainWindow.CasinoCanvas.TranslatePoint(new System.Windows.Point(0, 0), MainWindow);
            UpdateCasinoAreaData();
        }

        public Rect GetCurrentCasinoArea()
        {
            return new Rect(
                (MainWindow.Left + Settings.CasinoAreaWindowOffset.X),
                (MainWindow.Top + Settings.CasinoAreaWindowOffset.Y),
                Settings.CasinoAreaSize.Width,
                Settings.CasinoAreaSize.Height);
        }

        public Rect GetFixedCasinoArea()
        {
            var fixedPos = Settings.FixedCasinoWindowPosition;
            return new Rect(
                fixedPos.X,
                fixedPos.Y,
                Settings.CasinoAreaSize.Width,
                Settings.CasinoAreaSize.Height);
        }

        private void AdjustCasinoAreaBackground(bool transparent)
        {
            if (transparent)
            {
                MainWindow.Background = Settings.TransparentBackground;
                MainWindow.CasinoCanvas.Background = Settings.TransparentBackground;
            }
            else
            {
                MainWindow.Background = Settings.DefaultBackground;
                MainWindow.CasinoCanvas.Background = Settings.DefaultBackground;
            }
        }

        public void UpdateStatus(string message)
        {
            Status.Message = message;
        }

        public void AddNumberKindBalancePoint(NumberKind numKind, int advantage)
        {
            int newPointY = (MainWindow.NumberBalancesCanvasHeight / 2) - advantage;
            if (numKind == NumberKind.Red)
            {
                UpdateNumberBalances(RedBlackBalancesLine, newPointY);
            }
            else if (numKind == NumberKind.Even)
            {
                UpdateNumberBalances(EvenOddBalancesLine, newPointY);
            }
            else if (numKind == NumberKind.To18)
            {
                UpdateNumberBalances(To18From19BalancesLine, newPointY);
            }
        }

        private void UpdateNumberBalances(Polyline polyline, int newPointY)
        {
            Point curPoint = new Point(polyline.Points.Count, newPointY);
            polyline.Points.Add(curPoint);

            //offset line if too long
            var maxItemCount = MainWindow.NumberBalancesCanvasWidth;
            double offset = maxItemCount / 10;
            if (polyline.Points.Count >= maxItemCount)
            {
                polyline.BeginInit();

                for (int i = 0; i < offset; i++)
                    polyline.Points.RemoveAt(0);

                List<Point> newPoints = polyline.Points.Select(cur => new Point(cur.X - offset, cur.Y)).ToList();
                polyline.Points.Clear();
                foreach (var point in newPoints)
                    polyline.Points.Add(point);

                polyline.EndInit();
            }
        }


    }
}
