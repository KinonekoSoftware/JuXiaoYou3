using System.Collections.Concurrent;
using System.Globalization;
using System.IO;
using System.Reactive.Concurrency;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Acorisoft.FutureGL.MigaDB.Core;
using NLog;

namespace Acorisoft.FutureGL.MigaStudio.Resources.Converters
{
    public abstract class ImageConverterBase
    {
        private static IScheduler _scheduler;

        protected static IScheduler Scheduler => _scheduler ??= Xaml.Get<IScheduler>();
    }

    public abstract class ImageConverterBase<TEngine> : ImageConverterBase where TEngine : DataEngine
    {
        private static TEngine _engine;

        protected static TEngine Engine => _engine ??= Studio.Engine<TEngine>();

        protected virtual ImageSource Caching(string avatar, MemoryStream ms,int w,int h)
        {
            if (ms is null)
            {
                Xaml.Get<ILogger>()
                    .Warn($"图片:{avatar}为空！");
                return null;
            }
            
            return Xaml.FromStream(ms, w, h);
        }
    }

    // public class DirectiveAvatarConverter : IMultiValueConverter
    // {
    //     private static readonly BitmapImage _character = new BitmapImage(new Uri("pack://application:,,,/Forest.Fonts;component/avatar.png"));
    //
    //     private static ImageSource FallbackImage(DocumentType type)
    //     {
    //         return _character;
    //     }
    //
    //     public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    //     {
    //         if (values is null || values.Length == 0)
    //         {
    //             return FallbackImage(DocumentType.Character);
    //         }
    //
    //         if (values[1] is DocumentType type)
    //         {
    //             var avatar = values[0]?.ToString();
    //             if (string.IsNullOrEmpty(avatar))
    //             {
    //                 return FallbackImage(type);
    //             }
    //
    //
    //             return new BitmapImage(new Uri(avatar));
    //         }
    //
    //         return FallbackImage(DocumentType.Character);
    //     }
    //
    //     public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    //     {
    //         throw new NotSupportedException();
    //     }
    // }
}