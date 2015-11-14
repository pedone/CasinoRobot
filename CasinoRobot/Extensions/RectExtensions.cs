using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CasinoRobot.Extensions
{
    public static class RectExtensions
    {

        public static System.Drawing.Rectangle ToDrawingRectangle(this Rect rect)
        {
            return new System.Drawing.Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
        }

    }
}
