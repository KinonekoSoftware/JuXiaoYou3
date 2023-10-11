using System;
using System.Globalization;
using System.Windows.Data;

namespace Acorisoft.FutureGL.Tools.MusicPlayer
{
    public class TimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null) return ((TimeSpan)value).TotalSeconds;
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}