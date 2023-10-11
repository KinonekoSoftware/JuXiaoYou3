using System.Globalization;
using System.Windows.Data;

namespace Acorisoft.FutureGL.MigaStudio.Resources.Converters
{
    public class ReferenceSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // TODO: 翻译
            if (value is ReferenceSource kind)
            {
                return Language.Culture switch
                {
                    CultureArea.Chinese => GetChinese(kind),
                    _ => GetChinese(kind),
                };
            }

            return value?.ToString();
        }
        
        
        private static string GetChinese(ReferenceSource kind)
        {
            return kind switch
            {
                ReferenceSource.Document      => "设定",
                _                       => "剧情",
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}