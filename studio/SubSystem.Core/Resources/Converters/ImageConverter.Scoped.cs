using System.Collections.Concurrent;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Acorisoft.FutureGL.MigaStudio.Resources.Converters
{
    public class ScopedImageConverter : ImageConverterBase<ImageEngine>, IValueConverter, IMultiValueConverter, IDisposable
    {
       private  readonly ConcurrentDictionary<string, ImageSource> Pool      = new ConcurrentDictionary<string, ImageSource>();
        public  readonly BitmapImage                               Character = new BitmapImage(new Uri("pack://application:,,,/Forest.Fonts;component/avatar.png"));

        public void Reset() => Pool.Clear();
        

        private ImageSource Caching(string avatar)
        {
            if (!Pool.TryGetValue(avatar, out var img))
            {
                var ms = Engine.Get(avatar);
                img = Caching(avatar, ms, 256, 256);
                Pool.TryAdd(avatar, img);
            }

            return img;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values is null || values.Length == 0)
            {
                return Character;
            }

            var avatar = values[0]?.ToString();
            return string.IsNullOrEmpty(avatar) ? Character : Caching(avatar);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
            {
                return Character;
            }
            var avatar = value.ToString();
            return string.IsNullOrEmpty(avatar) ? Character : Caching(avatar);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                Reset();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ScopedImageConverter()
        {
            Dispose();
        }
    }
}