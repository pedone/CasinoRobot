using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CasinoRobot.ViewModels
{
    public class StatisticsLinesManager : INotifyPropertyChanged
    {

        public StatisticsViewModel Statistics
        {
            get { return ApplicationViewModel.Instance.Statistics; }
        }

        private int _Center;
        private System.Windows.Shapes.Polyline _WinningsStatisticsLine;
        public System.Windows.Shapes.Polyline WinningsStatisticsLine
        {
            get
            {
                return _WinningsStatisticsLine;
            }
            private set
            {
                _WinningsStatisticsLine = value;
                FirePropertyChanged("WinningsStatisticsLine");
            }
        }

        private System.Windows.Shapes.Polyline _NumberRepetitionsLine;
        public System.Windows.Shapes.Polyline NumberRepetitionsLine
        {
            get
            {
                return _NumberRepetitionsLine;
            }
            private set
            {
                _NumberRepetitionsLine = value;
                FirePropertyChanged("NumberRepetitionsLine");
            }
        }

        private System.Windows.Shapes.Polyline _NumberRepetitions2Line;
        public System.Windows.Shapes.Polyline NumberRepetitions2Line
        {
            get
            {
                return _NumberRepetitions2Line;
            }
            private set
            {
                _NumberRepetitions2Line = value;
                FirePropertyChanged("NumberRepetitions2Line");
            }
        }

        public int Center
        {
            get
            {
                return _Center;
            }
            set
            {
                _Center = value;
                ResetNumberRepetitionsLines();
            }
        }
        public int MaxWidth { get; set; }

        public StatisticsLinesManager()
        {
            Center = 200;
            MaxWidth = 1000;

            _WinningsStatisticsLine = new System.Windows.Shapes.Polyline
            {
                StrokeThickness = 2,
                Stroke = Brushes.Red,
                Visibility = Visibility.Collapsed
            };

            _NumberRepetitionsLine = new System.Windows.Shapes.Polyline
            {
                StrokeThickness = 2,
                Stroke = Brushes.Blue,
                Visibility = Visibility.Collapsed
            };

            _NumberRepetitions2Line = new System.Windows.Shapes.Polyline
            {
                StrokeThickness = 2,
                Stroke = Brushes.Red,
                Visibility = Visibility.Collapsed
            };
        }

        public void Update()
        {
            UpdateWinningsStatisticsLine();
            UpdateNumberRepetitionsLine();
            UpdateNumberRepetitions2Line();
        }

        private void UpdateWinningsStatisticsLine()
        {
            int newPointY = (int)(Center - Statistics.TotalWinnings);
            Point curPoint = new Point(WinningsStatisticsLine.Points.Count, newPointY);
            AddPoint(WinningsStatisticsLine, curPoint);
        }

        private void AddPoint(Polyline line, Point point)
        {
            line.Points.Add(point);

            //offset line if too long
            double offset = MaxWidth / 10;
            if (line.Points.Last().X > MaxWidth)
            {
                line.BeginInit();

                //for (int i = 0; i < offset; i++)
                //line.Points.RemoveAt(0);

                List<Point> newPoints = line.Points.Select(cur => new Point(cur.X - offset, cur.Y)).ToList();
                line.Points.Clear();
                foreach (var p in newPoints)
                    line.Points.Add(p);

                line.EndInit();
            }
        }

        private void ResetNumberRepetitionsLines()
        {
            if (NumberRepetitions2Line != null)
            {
                NumberRepetitions2Line.Points.Clear();
                if (NumberRepetitions2Line.Points.Count > 0)
                    NumberRepetitions2Line.Points.RemoveAt(0);

                NumberRepetitions2Line.Points.Add(new Point(0, Center));
            }
            if (NumberRepetitionsLine != null)
            {
                NumberRepetitionsLine.Points.Clear();
                if (NumberRepetitionsLine.Points.Count > 0)
                    NumberRepetitionsLine.Points.RemoveAt(0);

                NumberRepetitionsLine.Points.Add(new Point(5, Center));
            }
        }

        private void UpdateNumberRepetitions2Line()
        {
            if (ApplicationViewModel.Instance.Session.SpinCount == 0)
                return;

            const int positionOffset = 15;

            //after 36 () spins, money is lost
            double newPointY = Center;
            double newPointX = positionOffset;

            bool drawNewPoint = false;
            var lastPoint = NumberRepetitions2Line.Points.LastOrDefault();
            if (lastPoint != null)
            {
                newPointY = lastPoint.Y;
                newPointX = lastPoint.X + positionOffset;

                bool roundPassed = (ApplicationViewModel.Instance.Session.SpinCount % 36 == 0);
                if (roundPassed)
                    newPointY += positionOffset;

                bool isWin = (Statistics.NumberHistory.Count >= 3 && Statistics.NumberHistory[0] == Statistics.NumberHistory[2]);
                if (isWin)
                    newPointY -= positionOffset;

                drawNewPoint = isWin || roundPassed;
            }

            if (drawNewPoint)
                AddPoint(NumberRepetitions2Line, new Point(newPointX, newPointY));
        }

        private void UpdateNumberRepetitionsLine()
        {
            if (ApplicationViewModel.Instance.Session.SpinCount == 0)
                return;

            const int positionOffset = 15;

            //after 36 () spins, money is lost
            double newPointY = Center;
            double newPointX = positionOffset;

            bool drawNewPoint = false;
            var lastPoint = NumberRepetitionsLine.Points.LastOrDefault();
            if (lastPoint != null)
            {
                newPointY = lastPoint.Y;
                newPointX = lastPoint.X + positionOffset;

                bool roundPassed = (ApplicationViewModel.Instance.Session.SpinCount % 36 == 0);
                if (roundPassed)
                    newPointY += positionOffset;

                bool isWin = (Statistics.NumberHistory.Count >= 2 && Statistics.NumberHistory[0] == Statistics.NumberHistory[1]);
                if (isWin)
                    newPointY -= positionOffset;

                drawNewPoint = isWin || roundPassed;
            }

            if (drawNewPoint)
                AddPoint(NumberRepetitionsLine, new Point(newPointX, newPointY));
        }

        private void FirePropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
