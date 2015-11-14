using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using DPoint = System.Drawing.Point;
using DSize = System.Drawing.Size;

namespace CasinoRobot.Helpers
{
    public static class ScreenHelper
    {
        public static System.Drawing.Bitmap TakeScreenshot(Rect rect)
        {
            return TakeScreeshot(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static System.Drawing.Bitmap TakeScreeshot(double x, double y, double width, double height)
        {
            var screenshotBitmap = new System.Drawing.Bitmap((int)width, (int)height);
            using (var gfxScreenshot = System.Drawing.Graphics.FromImage(screenshotBitmap))
            {
                //gfxScreenshot.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                gfxScreenshot.CopyFromScreen(new DPoint((int)x, (int)y), new DPoint(0, 0), new DSize((int)width, (int)height), System.Drawing.CopyPixelOperation.SourceCopy);
            }

            return screenshotBitmap;
        }

        public static BitmapImage BitmapToImageSource(System.Drawing.Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

    }
}
