using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Acorisoft.FutureGL.MigaStudio.Resources.Converters
{

    public sealed class MuteOrUnmuteIconConverter : IValueConverter
    {
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = value != null && (bool)value;
            return val ? Mute : UnMute;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
        
        public Geometry Mute { get; set; }
        public Geometry UnMute { get; set; }
    }
}