using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CasinoRobot.Helpers
{
    public static class IntersectionHelper
    {
        public static bool Intersects(Point pointA1, Point pointA2, Point pointB1, Point pointB2)
        {
            double firstLineSlopeX, firstLineSlopeY, secondLineSlopeX, secondLineSlopeY;

            firstLineSlopeX = pointA2.X - pointA1.X;
            firstLineSlopeY = pointA2.Y - pointA1.Y;

            secondLineSlopeX = pointB2.X - pointB1.X;
            secondLineSlopeY = pointB2.Y - pointB1.Y;

            double s, t;
            s = (-firstLineSlopeY * (pointA1.X - pointB1.X) + firstLineSlopeX * (pointA1.Y - pointB1.Y)) / (-secondLineSlopeX * firstLineSlopeY + firstLineSlopeX * secondLineSlopeY);
            t = (secondLineSlopeX * (pointA1.Y - pointB1.Y) - secondLineSlopeY * (pointA1.X - pointB1.X)) / (-secondLineSlopeX * firstLineSlopeY + firstLineSlopeX * secondLineSlopeY);

            if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
            {
                double intersectionPointX = pointA1.X + (t * firstLineSlopeX);
                double intersectionPointY = pointA1.Y + (t * firstLineSlopeY);

                return true;
            }

            return false;
        }
    }
}
