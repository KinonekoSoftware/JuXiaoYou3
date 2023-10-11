using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Acorisoft.FutureGL.Forest;

namespace Acorisoft.FutureGL.Tools.MusicPlayer
{

    
    public sealed class PlayOrPauseIconConverter : IValueConverter
    {
        private Geometry _off;
        private Geometry _on;
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = value != null && (bool)value;
            if (val)
            {
                _off ??= Xaml.GetGeometry("Pause");
                return _off;
            }
            else
            {
                _on ??= Xaml.GetGeometry("Play");
                return _on;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}