using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace Jido.Converters
{
    public class RGBToColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is byte[] rgbBytes && rgbBytes.Length == 3)
            {
                return Color.FromRgb(rgbBytes[0], rgbBytes[1], rgbBytes[2]);
            }
            return Colors.White; // Default color if conversion fails
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is Color color)
            {
                return new byte[] { color.R, color.G, color.B };
            }
            return new byte[] { 0, 0, 0 }; // Default color if conversion fails
        }
    }
}
