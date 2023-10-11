using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Acorisoft.FutureGL.MigaStudio.Resources.Converters
{
    public class LockStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IDataCache cache)
            {
                return cache.IsLocked ? Lock : Unlock;
            }

            if(value is bool b)
            {
                return b ? Lock : Unlock;
            }

            return Unlock;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
        
        public Geometry Lock { get; set; }
        public Geometry Unlock { get; set; }
    }
    
    public class LockStateNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IDataCache cache)
            {
                return cache.IsLocked ? Lock : Unlock;
            }

            if(value is bool b)
            {
                return b ? Lock : Unlock;
            }

            return Unlock;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
        
        public string Lock { get; set; }
        public string Unlock { get; set; }
    }
}