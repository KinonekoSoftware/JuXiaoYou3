using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Acorisoft.FutureGL.Forest;

namespace Acorisoft.FutureGL.Tools.MusicPlayer
{

    public sealed class MuteOrUnmuteIconConverter : IValueConverter
    {
        private Geometry _on;
        private Geometry _off;
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = value != null && (bool)value;
            if (val)
            {
                _on ??= Xaml.GetGeometry("Mute");
                return _on;
            }
            else
            {
                _off ??= Xaml.GetGeometry("Volume");
                return _off;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}