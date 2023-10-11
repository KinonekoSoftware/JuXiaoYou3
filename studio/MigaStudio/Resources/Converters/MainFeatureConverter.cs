using System.Globalization;
using System.Windows.Data;

namespace Acorisoft.FutureGL.MigaStudio.Resources.Converters
{
    public class MainFeatureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MainFeature f)
            {
                return Language.GetText(f.NameId);
            }
            
            return Language.GetText(value?.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}