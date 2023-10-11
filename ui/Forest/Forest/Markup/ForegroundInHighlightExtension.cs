using System.Windows.Markup;

namespace Acorisoft.FutureGL.Forest.Markup
{
    public class ForegroundInHighlightExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ThemeSystem.Instance
                              .Theme
                              .Colors[(int)ForestTheme.ForegroundInHighlight];
        }
    }
    
    public class ForegroundInHighlightBrushExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ThemeSystem.Instance
                              .Theme
                              .Colors[(int)ForestTheme.ForegroundInHighlight]
                              .ToSolidColorBrush();
        }
    }
}