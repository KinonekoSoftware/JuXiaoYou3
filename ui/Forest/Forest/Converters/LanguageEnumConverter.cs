using System.Globalization;
using System.Windows.Data;
using Acorisoft.FutureGL.Forest.Services;

namespace Acorisoft.FutureGL.Forest.Converters
{
    public class LanguageEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum @enum)
            {
                return Language.GetEnum(@enum);
            }

            return $"无法翻译此类型, Type={value?.GetType()}, IsNull={value is null}, Value={value}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}