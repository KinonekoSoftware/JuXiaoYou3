using System.Globalization;
using System.Reactive.Concurrency;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;

namespace Acorisoft.FutureGL.MigaStudio.Resources.Converters
{
    public class ImageFromAlbumConverter : ImageConverterBase<ImageEngine>, IValueConverter
    {

        private ImageSource Caching(string avatar) => Caching(avatar, 1920, 1080);
        
        private ImageSource Caching(string avatar, int w, int h)
        {
            var ms  = Engine.Get(avatar);
            return Caching(avatar, ms, w, h);
        }
        

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
            {
                return null;
            }

            if (value is Album a)
            {
                return Caching(a.Source, a.Width, a.Height);
            }
            
            var avatar = value.ToString();
            return string.IsNullOrEmpty(avatar) ? null : Caching(avatar);
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}