using System.Drawing.Imaging;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Point = OpenCvSharp.Point;

namespace OpenCVSandbox.Tests
{
    internal static class ColorlessRectangleDetection
    {
        public static void Run()
        {
            // Create a window to display the screen capture
            Cv2.NamedWindow("ScreenCapture");

            // Set up the screen capture
            int counter = 0;
            while (true)
            {
                counter++;
                using (var screenImage = CaptureScreen())
                {
                    using Mat gray = new Mat();
                    Cv2.CvtColor(screenImage, gray, ColorConversionCodes.BGR2GRAY);
                    using Mat edges = new Mat();
                    Cv2.Canny(gray, edges, 10, 1000);
                    Cv2.FindContours(
                        edges,
                        out var contours,
                        out _,
                        RetrievalModes.List,
                        ContourApproximationModes.ApproxSimple
                    );
                    foreach (var contour in contours)
                    {
                        var perimeter = Cv2.ArcLength(contour, true);
                        var approx = Cv2.ApproxPolyDP(contour, 0.02 * perimeter, true);

                        // If the contour has 4 vertices, it's likely a rectangle
                        if (approx.Length == 4)
                        {
                            // Draw the contour
                            screenImage.DrawContours(new[] { contour }, -1, Scalar.Red, 2);
                        }
                    }
                    Cv2.ImShow("ScreenCapture", screenImage);
                    // Wait for a key press for a short time (10ms)
                    int key = Cv2.WaitKey(10);
                    if (key == (int)ConsoleKey.Escape) // Exit if the escape key is pressed
                        break;
                }
            }

            // Destroy the window
            Cv2.DestroyAllWindows();
        }

        public static Mat CaptureScreen()
        {
            Rectangle bounds = Screen.PrimaryScreen.Bounds;
            int width = bounds.Width / 2;
            int height = bounds.Height / 2;
            int x = bounds.X + width / 2;
            int y = bounds.Y + height / 2;
            Rectangle centerBounds = new Rectangle(x, y, width, height);
            // Create a Bitmap object to hold the screen capture
            Bitmap screenshot = new Bitmap(centerBounds.Width, centerBounds.Height, PixelFormat.Format32bppRgb);

            // Capture the screen into the Bitmap object
            using (Graphics graphics = Graphics.FromImage(screenshot))
            {
                graphics.CopyFromScreen(
                    centerBounds.X,
                    centerBounds.Y,
                    0,
                    0,
                    centerBounds.Size,
                    CopyPixelOperation.SourceCopy
                );
            }
            Mat mat = screenshot.ToMat();
            screenshot.Dispose();
            return mat;
        }
    }
}
