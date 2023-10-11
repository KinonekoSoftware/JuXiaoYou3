using System.Globalization;
using System.Windows.Data;

namespace Acorisoft.FutureGL.Forest.Converters
{
    #region BooleanToVisibility

    public class BooleanTrueToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (ReferenceEquals(Boxing.True, value))
            {
                return Visibility.Visible;
            }

            if (ReferenceEquals(Boxing.False, value))
            {
                return Visibility.Collapsed;
            }

            if (value is bool b && b)
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility v)
            {
                return v == Visibility.Visible;
            }

            return false;
        }
    }
    
    public class BooleanFalseToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (ReferenceEquals(Boxing.True, value))
            {
                return Visibility.Collapsed;
            }

            if (ReferenceEquals(Boxing.False, value))
            {
                return Visibility.Visible;
            }

            if (value is bool b && !b)
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility v)
            {
                return v != Visibility.Visible;
            }

            return true;
        }
    }
    #endregion
}