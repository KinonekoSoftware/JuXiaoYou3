using System.Globalization;
using System.Windows.Data;

namespace Acorisoft.FutureGL.MigaStudio.Converters
{
    public class IsLessThanConverter : DependencyObject,IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var param = System.Convert.ToDouble(parameter);
            var width = System.Convert.ToDouble(value);

            if (width > 0 && width < param)
                return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
