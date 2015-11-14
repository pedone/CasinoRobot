using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CasinoRobot.Extensions
{
    public static class PointExtensions
    {

        public static System.Drawing.Point ToDrawingPoint(this Point point)
        {
            return new System.Drawing.Point((int)point.X, (int)point.Y);
        }

    }
}
