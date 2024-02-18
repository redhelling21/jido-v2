using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace Jido.Utils
{
    public static class ScreenUtils
    {
        public static Mat CaptureScreen(Rectangle bounds)
        {
            // Create a Bitmap object to hold the screen capture
            Bitmap screenshot = new(bounds.Width, bounds.Height, PixelFormat.Format32bppRgb);

            // Capture the screen into the Bitmap object
            using (Graphics graphics = Graphics.FromImage(screenshot))
            {
                graphics.CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size, CopyPixelOperation.SourceCopy);
            }
            Mat mat = screenshot.ToMat();
            screenshot.Dispose();
            return mat;
        }
    }
}
