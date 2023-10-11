using System;
using System.Globalization;
using System.Windows.Data;
using ImTools;

namespace Acorisoft.FutureGL.MigaStudio.Inspirations.Resources.Converters
{
    public class ContractNameConverter : IMultiValueConverter
    {
        private const string Pattern = "{0} ({1})";
        
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values is null)
            {
                return "null";
            }

            var raw   = values[0]?.ToString();
            var alias = values[1]?.ToString();

            if (string.IsNullOrEmpty(raw))
            {
                return Language.GetText("global.unNamed");
            }

            return string.IsNullOrEmpty(alias) ? raw : string.Format(Pattern, alias, raw);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}