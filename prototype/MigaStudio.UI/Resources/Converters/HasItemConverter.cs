using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace Acorisoft.FutureGL.MigaStudio.Resources.Converters
{
    public class HasItemConverter : IValueConverter
    {
        public static readonly HasItemConverter Instance = new HasItemConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable ie)
            {
                var r = ie.GetEnumerator()
                          .MoveNext();

                return Boxing.Box(r);
            }

            return Boxing.False;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}