using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;

namespace Acorisoft.FutureGL.MigaStudio.Resources.Converters
{
    
    
    public class ImageFromScaledStringConverter :ImageConverterBase<ImageEngine>, IValueConverter
    {
        private ImageSource Caching(string avatar, int w, int h)
        {
            w = Math.Clamp(w, 100, 2560);
            h = Math.Clamp(h, 100, 1440);
            var ms  = Engine.Get(avatar);
            var img = Caching(avatar, ms, w, h);
            return img;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
            {
                return null;
            }

            var v = value.ToString();
            
            if (string.IsNullOrEmpty(v))
            {
                return null;
            }
            
            if (value is Album a)
            {
                return Caching(a.Source, a.Width, a.Height);
            }

            var values = v.Split(":");

            if (values.Length < 3)
            {

                return null;
            }
            
            var avatar = values[0];
            var w      = int.TryParse(values[1], out var n) ? n : 100;
            var h      = int.TryParse(values[2], out n) ? n : 100;
            return string.IsNullOrEmpty(avatar) ? null : Caching(avatar, w, h);
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}