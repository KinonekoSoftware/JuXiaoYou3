using System;
using System.Globalization;
using System.Windows.Data;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Tools.MusicPlayer.Services;

namespace Acorisoft.FutureGL.Tools.MusicPlayer
{
    public class PlayModeIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PlayMode mode)
            {
                return mode switch
                {
                    PlayMode.Loop => Xaml.GetGeometry("Loop"),
                    PlayMode.Repeat => Xaml.GetGeometry("Repeat"),
                    PlayMode.Shuffle => Xaml.GetGeometry("Shuffle"),
                    _ => Xaml.GetGeometry("Sequence")
                };
            }

            return Xaml.GetGeometry("Sequence");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}