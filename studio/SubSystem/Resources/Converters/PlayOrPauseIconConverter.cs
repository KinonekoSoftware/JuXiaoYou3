using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Acorisoft.FutureGL.MigaStudio.Resources.Converters
{

    
    public sealed class PlayOrPauseIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = value != null && (bool)value;
            return val ? Pause : Play;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
        
        public Geometry Play { get; set; }
        public Geometry Pause { get; set; }
    }
}