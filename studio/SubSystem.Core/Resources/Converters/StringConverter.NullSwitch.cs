using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Acorisoft.FutureGL.MigaStudio.Resources.Converters
{
    public class NullSwitchStringConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values is null || values.Length == 0)
            {
                return string.Empty;
            }


            return values.Where(x => x is not null)
                         .Select(x =>
                         {
                             var s = x.ToString() ?? string.Empty;
                             var len = Math.Clamp(s.Length, 0, 30);
                             return s.Substring(0, len);
                         })
                         .First(x => !string.IsNullOrEmpty(x));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}