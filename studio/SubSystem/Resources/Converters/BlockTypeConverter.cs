using System.Globalization;
using System.Windows.Data;

namespace Acorisoft.FutureGL.MigaStudio.Resources.Converters
{
    public class BlockTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BlockType kind)
            {
                
                return SubSystemString.GetModuleBlockNameByKind(kind);
            }

            return value?.ToString() ?? "Null";
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}