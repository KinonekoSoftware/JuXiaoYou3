using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Acorisoft.FutureGL.MigaStudio.Resources.Converters
{
    public class PlayModeIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PlayMode mode)
            {
                return mode switch
                {
                    PlayMode.Loop    => Loop,
                    PlayMode.Repeat  => Repeat,
                    PlayMode.Shuffle => Shuffle,
                    _                => Sequence
                };
            }

            return Sequence;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
        
        public Geometry Loop { get; set; }
        public Geometry Repeat { get; set; }
        public Geometry Shuffle { get; set; }
        public Geometry Sequence { get; set; }
    }
}