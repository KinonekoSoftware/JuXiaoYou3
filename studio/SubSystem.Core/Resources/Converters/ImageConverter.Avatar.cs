using System.Collections.Concurrent;
using System.Globalization;
using System.Reactive.Concurrency;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Acorisoft.FutureGL.MigaStudio.Resources.Converters
{
    public class AvatarConverter : ImageConverterBase<ImageEngine>, IMultiValueConverter, IValueConverter
    {
        private static readonly ConcurrentDictionary<string, ImageSource> Pool = new ConcurrentDictionary<string, ImageSource>();
        public static readonly  BitmapImage                               Character = new BitmapImage(new Uri("pack://application:,,,/Forest.Fonts;component/avatar.png"));

        public static void Reset() => Pool.Clear();
        
        private static ImageSource FallbackImage(DocumentType type)
        {
            return Character;
        }

        private ImageSource Caching(string avatar)
        {
            if (!Pool.TryGetValue(avatar, out var img))
            {
                var ms = Engine.Get(avatar);
                img = Caching(avatar, ms, 256, 256) ?? Character;
                Pool.TryAdd(avatar, img);
            }

            return img;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values is null || values.Length == 0)
            {
                return FallbackImage(DocumentType.Character);
            }

            if (values[1] is DocumentType type)
            {
                var avatar = values[0]?.ToString();
                return string.IsNullOrEmpty(avatar) ? FallbackImage(type) : Caching(avatar);
            }

            return FallbackImage(DocumentType.Character);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
            {
                return FallbackImage(DocumentType.Character);
            }
            
            var avatar = value?.ToString();
            
            
            return string.IsNullOrEmpty(avatar) ? FallbackImage(DocumentType.Character) : Caching(avatar);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}