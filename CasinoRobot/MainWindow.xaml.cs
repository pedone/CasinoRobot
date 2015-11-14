using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Point = System.Windows.Point;
using DrawingPoint = System.Drawing.Point;
using System.IO;
using System.Drawing.Imaging;
using System.Xml.Serialization;
using System.Timers;
using System.Threading;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using WindowsInput;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Threading;
using CasinoRobot.ViewModels;
using System.Collections.Specialized;
using CasinoRobot.Helpers;

namespace CasinoRobot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public double NumberBalancesCanvasWidth
        {
            get { return cNumberKindBalances.ActualWidth; }
        }

        public int NumberBalancesCanvasHeight
        {
            get { return (int)cNumberKindBalances.ActualHeight; }
        }

        public int StatisticsLinesCanvasWidth
        {
            get { return (int)cStatisticsLines.ActualWidth; }
        }

        public int StatisticsLinesCanvasHeight
        {
            get { return (int)cStatisticsLines.ActualHeight; }
        }

        private bool _IsMovingWindow = false;
        private System.Windows.Point _MoveWindowStartPos;

        public ApplicationViewModel ApplicationView
        {
            get { return ApplicationViewModel.Instance; }
        }

        public MainWindow()
        {
            DataContext = ApplicationViewModel.Instance;
            InitializeComponent();

            CasinoCanvas.MouseWheel += CasinoCanvas_MouseWheel_PanCanvas;
            CasinoCanvas.MouseMove += CasinoCanvas_MouseMove_PanCanvas;
            CasinoCanvas.MouseDown += CasinoCanvas_MouseMove_PanCanvas;
            CasinoCanvas.MouseDown += CasinoCanvas_MouseDown;
            CasinoCanvas.MouseUp += CasinoCanvas_MouseUp_PanCanvas;

            PreviewMouseMove += MoveWindow_MouseMove;
            PreviewMouseUp += MoveWindow_MouseUp;
            PreviewKeyDown += MainWindow_PreviewKeyDown;

            Loaded += MainWindow_Loaded;
            cStatisticsLines.SizeChanged += cStatisticsLines_SizeChanged;

            ((INotifyCollectionChanged)ApplicationView.Statistics.BetHistory).CollectionChanged += BetHistory_CollectionChanged;
            ((INotifyCollectionChanged)ApplicationView.Statistics.NumberBetHistory).CollectionChanged += NumberBetHistory_CollectionChanged;
        }

        void cStatisticsLines_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            InitStatisticsLineCanvas();
            ApplicationView.Statistics.StatisticsLines.Center = StatisticsLinesCanvasHeight / 2;
            ApplicationView.Statistics.StatisticsLines.MaxWidth = StatisticsLinesCanvasWidth;
        }

        private void InitStatisticsLineCanvas()
        {
            //Init Number Kind Balances
            //draw horizontal center line
            Line centerLine = new Line
            {
                Stroke = new SolidColorBrush(Colors.Gray),
                X1 = 0,
                X2 = 2000,
                Y1 = NumberBalancesCanvasHeight / 2,
                Y2 = NumberBalancesCanvasHeight / 2,
                StrokeDashArray = { 2, 2 }
            };

            cNumberKindBalances.Children.Clear();
            cNumberKindBalances.Children.Add(centerLine);
            cNumberKindBalances.Children.Add(ApplicationView.RedBlackBalancesLine);
            cNumberKindBalances.Children.Add(ApplicationView.EvenOddBalancesLine);
            cNumberKindBalances.Children.Add(ApplicationView.To18From19BalancesLine);

            //Init Statistics Lines Canvas
            //draw horizontal center line
            Line centerLine2 = new Line
            {
                Stroke = new SolidColorBrush(Colors.Gray),
                X1 = 0,
                X2 = 2000,
                Y1 = StatisticsLinesCanvasHeight / 2,
                Y2 = StatisticsLinesCanvasHeight / 2,
                StrokeDashArray = { 2, 2 }
            };

            cStatisticsLines.Children.Clear();
            cStatisticsLines.Children.Add(centerLine2);
            cStatisticsLines.Children.Add(ApplicationView.Statistics.StatisticsLines.WinningsStatisticsLine);
            cStatisticsLines.Children.Add(ApplicationView.Statistics.StatisticsLines.NumberRepetitionsLine);
            cStatisticsLines.Children.Add(ApplicationView.Statistics.StatisticsLines.NumberRepetitions2Line);
        }

        void BetHistory_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            lvBetHistory.SelectedIndex = lvBetHistory.Items.Count - 1;
            lvBetHistory.ScrollIntoView(lvBetHistory.SelectedItem);
            lvBetHistory.SelectedIndex = -1;
        }

        void NumberBetHistory_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            lvNumberBetHistory.SelectedIndex = lvNumberBetHistory.Items.Count - 1;
            lvNumberBetHistory.ScrollIntoView(lvNumberBetHistory.SelectedItem);
            lvNumberBetHistory.SelectedIndex = -1;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Width = ApplicationViewModel.Instance.Settings.WindowWidth;
            Height = ApplicationViewModel.Instance.Settings.WindowHeight;

            ApplicationViewModel.Instance.Init();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            ApplicationView.ShowSettings = false;
            ApplicationView.Session.Start();
        }

        private void CasinoCanvas_MouseUp_PanCanvas(object sender, MouseButtonEventArgs e)
        {
            if (!ApplicationView.IsEditingMarkers)
                return;

            ApplicationView.LastCanvasTranslationPosition = null;
        }

        private void CasinoCanvas_MouseMove_PanCanvas(object sender, MouseButtonEventArgs e)
        {
            if (!ApplicationView.IsEditingMarkers)
                return;

            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                ApplicationView.LastCanvasTranslationPosition = e.GetPosition(CasinoCanvas);
            }
        }

        void CasinoCanvas_MouseMove_PanCanvas(object sender, MouseEventArgs e)
        {
            if (!ApplicationView.IsEditingMarkers)
                return;

            if (e.MiddleButton == MouseButtonState.Pressed && ApplicationView.LastCanvasTranslationPosition != null)
            {
                var element = sender as UIElement;
                var position = e.GetPosition(element);
                var transform = (MatrixTransform)element.RenderTransform;
                var matrix = transform.Matrix;
                var offset = position - ApplicationView.LastCanvasTranslationPosition.Value;

                matrix.TranslatePrepend(offset.X, offset.Y);
                transform.Matrix = matrix;
            }
        }

        void CasinoCanvas_MouseWheel_PanCanvas(object sender, MouseWheelEventArgs e)
        {
            if (!ApplicationView.IsEditingMarkers)
                return;

            //scale the casino canvas
            var element = sender as UIElement;
            var position = e.GetPosition(element);
            var transform = (MatrixTransform)element.RenderTransform;
            var matrix = transform.Matrix;
            var scale = e.Delta >= 0 ? 1.1 : (1.0 / 1.1);

            matrix.ScaleAtPrepend(scale, scale, position.X, position.Y);
            transform.Matrix = matrix;
        }

        private void CasinoCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ApplicationView.CurrentlyAssigningMarker == null)
                return;

            bool isRightClick = e.ChangedButton == System.Windows.Input.MouseButton.Right;
            bool isMiddleClick = e.ChangedButton == System.Windows.Input.MouseButton.Middle;
            var curPosition = e.GetPosition(CasinoCanvas);
            var roundedPosition = new Point(Math.Round(curPosition.X), Math.Round(curPosition.Y));

            if (ApplicationView.CurrentlyAssigningMarker is CasinoButtonViewModel)
            {
                if (isRightClick)
                    ((CasinoButtonViewModel)ApplicationView.CurrentlyAssigningMarker).Position = null;
                else
                    ((CasinoButtonViewModel)ApplicationView.CurrentlyAssigningMarker).Position = roundedPosition;

                ApplicationView.CurrentlyAssigningMarker = null;
            }
            else if (ApplicationView.CurrentlyAssigningMarker is CasinoBetButtonViewModel)
            {
                if (isRightClick)
                    ((CasinoBetButtonViewModel)ApplicationView.CurrentlyAssigningMarker).Position = null;
                else
                    ((CasinoBetButtonViewModel)ApplicationView.CurrentlyAssigningMarker).Position = roundedPosition;

                ApplicationView.CurrentlyAssigningMarker = null;
            }
            else if (ApplicationView.CurrentlyAssigningMarker is CasinoNumberViewModel)
            {
                if (isMiddleClick)
                    ApplicationView.CurrentlyAssigningMarker = null;
                else if (isRightClick)
                    ((CasinoNumberViewModel)ApplicationView.CurrentlyAssigningMarker).RemoveLastAreaPosition();
                else
                    ((CasinoNumberViewModel)ApplicationView.CurrentlyAssigningMarker).Area.Add(roundedPosition);
            }

            e.Handled = true;
        }

        private void MoveWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                _MoveWindowStartPos = e.GetPosition(this);
                Mouse.Capture(this);
                Mouse.OverrideCursor = Cursors.None;

                _IsMovingWindow = true;
            }
        }

        private void MoveWindow_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_IsMovingWindow)
            {
                Mouse.Capture(null);
                Mouse.OverrideCursor = null;

                _IsMovingWindow = false;
            }
        }

        void MoveWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (_IsMovingWindow)
            {
                var curPos = e.GetPosition(this);
                var movement = _MoveWindowStartPos - curPos;

                Left -= movement.X;
                Top -= movement.Y;
            }
        }

        void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            AssignRefreshMarker(e);

            if (!ApplicationView.IsAdjustingCasinoArea)
                return;

            if (e.Key == Key.Left)
                Left -= 1;
            else if (e.Key == Key.Right)
                Left += 1;
            else if (e.Key == Key.Up)
                Top -= 1;
            else if (e.Key == Key.Down)
                Top += 1;
            else if (e.Key == Key.Return || e.Key == Key.Escape)
                ApplicationView.IsAdjustingCasinoArea = false;

            e.Handled = true;
        }

        private void AssignRefreshMarker(KeyEventArgs e)
        {
            var refreshMarker = ApplicationView.CurrentlyAssigningMarker as CasinoButtonViewModel;
            if (refreshMarker == null || refreshMarker.Kind != RouletteButtonKind.RefreshPosition)
                return;

            if (e.Key == Key.LeftCtrl)
            {
                var curPosition = MouseHelper.GetMousePosition();
                var roundedPosition = new Point(Math.Round(curPosition.X), Math.Round(curPosition.Y));

                refreshMarker.Position = roundedPosition;
                ApplicationView.CurrentlyAssigningMarker = null;
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            ApplicationView.Session.Stop();
        }

        private void LoadSettings_Click(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(ApplicationView.Settings.DefaultDirectory))
                Directory.CreateDirectory(ApplicationView.Settings.DefaultDirectory);

            OpenFileDialog openDialog = new OpenFileDialog
            {
                RestoreDirectory = true,
                InitialDirectory = ApplicationView.Settings.DefaultDirectory,
                Filter = "XML (*.xml)|*.xml|All Files (*.*)|*.*"
            };

            if (openDialog.ShowDialog() == true)
            {
                using (FileStream fileStream = new FileStream(openDialog.FileName, FileMode.Open))
                    ApplicationView.Settings.Load(fileStream);
            }
        }

        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(ApplicationView.Settings.DefaultDirectory))
                Directory.CreateDirectory(ApplicationView.Settings.DefaultDirectory);

            SaveFileDialog saveDialog = new SaveFileDialog
            {
                RestoreDirectory = true,
                Filter = "XML (*.xml)|*.xml|All Files (*.*)|*.*",
                InitialDirectory = ApplicationView.Settings.DefaultDirectory,
                FileName = "CasinoSettings.xml"
            };

            if (saveDialog.ShowDialog() == true)
            {
                using (FileStream fileStream = new FileStream(saveDialog.FileName, FileMode.Create))
                    ApplicationView.Settings.Save(fileStream);
            }
        }

        private void ResetStatistics_Click(object sender, RoutedEventArgs e)
        {
            ApplicationView.Statistics.ResetBetHistory();
            ApplicationView.Statistics.ResetStreaks();
            ApplicationView.Statistics.ResetMaxStreakCounts();
            ApplicationView.Statistics.ResetNumberHistory();
            ApplicationView.Statistics.ResetStreaksWinningsAndLosses();
            ApplicationView.Statistics.ResetBalance();
        }

    }
}
