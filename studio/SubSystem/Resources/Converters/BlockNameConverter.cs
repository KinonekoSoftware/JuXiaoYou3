using System.Globalization;
using System.Windows.Data;

namespace Acorisoft.FutureGL.MigaStudio.Resources.Converters
{
    public class BlockNameConverter : IValueConverter, IMultiValueConverter
    {
        private const string Pattern = "{0}: {1}";

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var t = values[0] as ModuleBlockEditUI;
            var n = values[1]?.ToString();

            if (string.IsNullOrEmpty(t?.Name))
            {
                return n;
            }

            if (t is BinaryBlockEditUI d)
            {
                return $"{d.Positive}-{d.Negative}";
            }
            return t.Name;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BlockType bt)
            {
                var raw = SubSystemString.GetModuleBlockNameByKind(bt);
                return Language.GetText(raw);
            }
                
            

            if (value is BinaryBlockEditUI d)
            {
                return $"{d.Positive}-{d.Negative}";
            }
            
            return SubSystemString.GetModuleBlockNameByType(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}