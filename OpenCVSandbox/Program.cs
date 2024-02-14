// See https://aka.ms/new-console-template for more information
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Point = OpenCvSharp.Point;

internal class Program
{
    private static void Main(string[] args)
    {
        // Create a window to display the screen capture
        Cv2.NamedWindow("ScreenCapture");

        // Set up the screen capture
        while (true)
        {
            using (var t = new ResourcesTracker())
            {
                // Capture the screen
                var screenImage = CaptureScreen();
                var result = TestingGround(screenImage);
                // Display the captured screen in the window
                Cv2.ImShow("ScreenCapture", result);

                // Wait for a key press for a short time (10ms)
                int key = Cv2.WaitKey(30);
                if (key == (int)ConsoleKey.Escape) // Exit if the escape key is pressed
                    break;
            }
        }

        // Destroy the window
        Cv2.DestroyAllWindows();
    }

    public static Mat TestingGround(Mat origin)
    {
        using (var t = new ResourcesTracker())
        {
            Console.WriteLine(origin.Channels());
            Console.WriteLine(origin.GetType());
            Cv2.CvtColor(origin, origin, ColorConversionCodes.BGRA2RGB);
            Mat thresh = new Mat();
            Scalar lowerBound = new Scalar(253, 0, 253); // Adjusted lower bound
            Scalar upperBound = new Scalar(253, 20, 253); // Adjusted upper bound
            Cv2.InRange(origin, lowerBound, upperBound, thresh);
            //Cv2.Threshold(thresh, thresh, 0, 255, ThresholdTypes.Binary);
            return thresh;
        }
    }

    public static Mat CaptureScreen()
    {
        Rectangle bounds = Screen.PrimaryScreen.Bounds;

        // Create a Bitmap object to hold the screen capture
        Bitmap screenshot = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb);

        // Capture the screen into the Bitmap object
        using (Graphics graphics = Graphics.FromImage(screenshot))
        {
            graphics.CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size, CopyPixelOperation.SourceCopy);
        }

        return BitmapConverter.ToMat(screenshot);
    }
}
