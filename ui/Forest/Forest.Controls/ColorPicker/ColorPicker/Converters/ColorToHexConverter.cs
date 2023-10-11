using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Data;
using Acorisoft.FutureGL.MigaUtils;

namespace ColorPicker.Converters
{
    [ValueConversion(typeof(Color), typeof(string))]
    internal class ColorToHexConverter : DependencyObject, IValueConverter
    {
        private const char HexPrefix = '#';
        private const string HexPrefixWithFF = "#FF";
        
        public static readonly DependencyProperty ShowAlphaProperty =
            DependencyProperty.Register(nameof(ShowAlpha), typeof(bool), typeof(ColorToHexConverter),
                new PropertyMetadata(true, ShowAlphaChangedCallback));

        public bool ShowAlpha
        {
            get => (bool)GetValue(ShowAlphaProperty);
            set => SetValue(ShowAlphaProperty, value);
        }

        public event EventHandler OnShowAlphaChange;

        public void RaiseShowAlphaChange()
        {
            OnShowAlphaChange(this, EventArgs.Empty);
        }

        private static void ShowAlphaChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ColorToHexConverter)d).RaiseShowAlphaChange();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !ShowAlpha ? ConvertNoAlpha(value) : ((Color)value).ToString();
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ShowAlpha)
                return ConvertBackNoAlpha(value);
            var text = (string)value;
            text = Regex.Replace(text.ToUpperInvariant(), @"[^0-9A-F]", "");
            var final =Pool.GetStringBuilder();
            if (text.Length == 3) //short hex with no alpha
            {
                final.Append(HexPrefixWithFF)
                     .Append(text[0])
                     .Append(text[0])
                     .Append(text[1])
                     .Append(text[1])
                     .Append(text[2])
                     .Append(text[2]);
            }
            else if (text.Length == 4) //short hex with alpha
            {
                final.Append(HexPrefix)
                     .Append(text[0])
                     .Append(text[0])
                     .Append(text[1])
                     .Append(text[1])
                     .Append(text[2])
                     .Append(text[2])
                     .Append(text[3])
                     .Append(text[3]);
            }
            else if (text.Length == 6) //hex with no alpha
            {
                final.Append(HexPrefixWithFF)
                     .Append(text);
            }
            else
            {
                final.Append(HexPrefix)
                     .Append(text);
            }
            try
            {
                var s = final.ToString();
                Pool.ReturnStringBuilder(final);
                return ColorConverter.ConvertFromString(s);
            }
            catch (Exception)
            {
                return DependencyProperty.UnsetValue;
            }
        }

        public static object ConvertNoAlpha(object value)
        {
            return HexPrefix + ((Color)value).ToString()
                                             .Substring(3, 6);
        }

        public static object ConvertBackNoAlpha(object value)
        {
            var text = (string)value;
            text = Regex.Replace(text.ToUpperInvariant(), @"[^0-9A-F]", "");
            var final = new StringBuilder();
            if (text.Length == 3) //short hex
            {
                final.Append(HexPrefixWithFF).Append(text[0]).Append(text[0]).Append(text[1]).Append(text[1]).Append(text[2]).Append(text[2]);
            }
            else if (text.Length == 4)
            {
                return DependencyProperty.UnsetValue;
            }
            else if (text.Length > 6)
            {
                return DependencyProperty.UnsetValue;
            }
            else //regular hex
            {
                final.Append(HexPrefix).Append(text);
            }
            try
            {
                return ColorConverter.ConvertFromString(final.ToString());
            }
            catch (Exception)
            {
                return DependencyProperty.UnsetValue;
            }
        }
    }
}
