using System.Globalization;
using System.Windows.Data;

namespace Acorisoft.FutureGL.MigaStudio.Resources.Converters
{
    public class PageIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = value?.ToString() ?? "1";
            return Math.Clamp(int.TryParse(v, out var n) ? n : 1, 1, 500);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString() ?? "1";
        }
    }
}