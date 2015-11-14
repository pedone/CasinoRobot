using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using CasinoRobot.Extensions;
using System.Diagnostics;
using CasinoRobot.Helpers;

namespace CasinoRobot.ViewModels
{
    public class CasinoNumberViewModel : MarkerViewModel
    {

        private const int ScreenshotDifferencePixelCountThreshold = 30;
        /// <summary>
        /// because zero has twice as many pixels, the pixel threshold needs to be higher or zero might be accidentally selected
        /// </summary>
        private const int ScreenshotDifferencePixelZeroCountOffset = 15;

        public int Number { get; set; }

        public ObservableCollection<Point> Area { get; private set; }

        public Rect Bounds { get; private set; }

        public Point Center
        {
            get
            {
                return new Point((int)(Bounds.X + (Bounds.Width / 2)),
                    (int)(Bounds.Y + (Bounds.Height / 2)));
            }
        }

        private List<ComparisonPixel> ComparisonPixels { get; set; }
        private ComparisonPixel CenterPixel { get; set; }

        public CasinoNumberViewModel()
        {
            Area = new ObservableCollection<Point>();
            Area.CollectionChanged += Area_CollectionChanged;
        }

        void Area_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            HasValue = Area.Count >= 2;
            if (HasValue)
                UpdateBounds();

            FirePropertyChanged("Area");
        }

        private void UpdateBounds()
        {
            var xMin = Area.Select(cur => cur.X).Min();
            var xMax = Area.Select(cur => cur.X).Max();
            var yMin = Area.Select(cur => cur.Y).Min();
            var yMax = Area.Select(cur => cur.Y).Max();

            Point topLeft = new Point(xMin, yMin);
            Point bottomRight = new Point(xMax, yMax);
            Bounds = new Rect(topLeft, bottomRight);
        }

        public bool ContainsPosition(Point position)
        {
            if (!HasValue)
                return false;

            var pointA1 = Center;
            var pointA2 = position;

            //area contains position when center to position does not intersect any area bounds
            for (int i = -1; i < Area.Count - 1; i++)
            {
                Point pointB1;
                if (i == -1)
                    pointB1 = Area.Last();
                else
                    pointB1 = Area[i];

                Point pointB2 = Area[i + 1];
                if (IntersectionHelper.Intersects(pointA1, pointA2, pointB1, pointB2))
                    return false;
            }

            return true;
        }

        internal void RemoveLastAreaPosition()
        {
            if (Area.Count > 0)
                Area.RemoveAt(Area.Count - 1);
        }

        public void InitComparisonPixels()
        {
            ComparisonPixels = new List<ComparisonPixel>();
            var originalScreenshot = ApplicationViewModel.Instance.Settings.CasinoScreenshot;

            //add center pixel
            var centerDPoint = Center.ToDrawingPoint();
            CenterPixel = new ComparisonPixel
            {
                Position = Center,
                Color = originalScreenshot.GetPixel(centerDPoint.X, centerDPoint.Y)
            };

            for (int x = (int)Bounds.Left; x < (int)Bounds.Right; x += 5)
                for (int y = (int)Bounds.Top; y < (int)Bounds.Bottom; y += 5)
                {
                    var curPosition = new Point(x, y);
                    if (ContainsPosition(curPosition))
                    {
                        var dpoint = curPosition.ToDrawingPoint();
                        ComparisonPixels.Add(new ComparisonPixel
                        {
                            Position = curPosition,
                            Color = originalScreenshot.GetPixel(dpoint.X, dpoint.Y)
                        });
                    }
                }
        }

        internal bool IsDrawn(System.Drawing.Bitmap curCasinoScreenshot, System.Drawing.Bitmap originalCasinoScreenshot)
        {
            bool passedRoughCheck = false;
            //do a rough check first, only check the center and every 10th pixel
            if (!MatchesComparisonPixel(CenterPixel, curCasinoScreenshot, originalCasinoScreenshot))
                passedRoughCheck = true;
            else
                for (int i = 0; i < ComparisonPixels.Count; i += 10)
                {
                    if (!MatchesComparisonPixel(ComparisonPixels[i], curCasinoScreenshot, originalCasinoScreenshot))
                    {
                        passedRoughCheck = true;
                        break;
                    }
                }

            if (!passedRoughCheck)
                return false;

            //do a thorough check: check every base comparison pixel
            int unmatchingPixelCount = 0;
            foreach (var basePixel in ComparisonPixels)
            {
                if (!MatchesComparisonPixel(basePixel, curCasinoScreenshot, originalCasinoScreenshot))
                    unmatchingPixelCount++;
            }

            //if there's too many unmatching pixels, it can't be the number, probably the "play for real money now" message
            if (unmatchingPixelCount >= ComparisonPixels.Count * 0.9)
                return false;

            if (Number != 0)
                return unmatchingPixelCount >= ScreenshotDifferencePixelCountThreshold;
            else
                return unmatchingPixelCount >= (ScreenshotDifferencePixelCountThreshold + ScreenshotDifferencePixelZeroCountOffset);

        }

        private bool MatchesComparisonPixel(ComparisonPixel basePixel, System.Drawing.Bitmap casinoScreenshot, System.Drawing.Bitmap originalCasinoScreenshot)
        {
            //TODO use color difference algorithm

            var dPosition = basePixel.Position.ToDrawingPoint();
            var pixelColor = casinoScreenshot.GetPixel(dPosition.X, dPosition.Y);

            return basePixel.Color.Equals(pixelColor);
        }

        internal List<ComparisonPixel> GetComparisonPixels()
        {
            return ComparisonPixels.ToList();
        }
    }
}
