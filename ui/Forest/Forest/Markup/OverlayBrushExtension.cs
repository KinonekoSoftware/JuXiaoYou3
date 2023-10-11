using System.Windows.Markup;

namespace Acorisoft.FutureGL.Forest.Markup
{
    public class OverlayBrushExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ThemeSystem.Instance
                              .Theme
                              .Colors[(int)ForestTheme.Overlay100]
                              .ToSolidColorBrush();
        }
    }
    
    public class OverlayExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ThemeSystem.Instance
                              .Theme
                              .Colors[(int)ForestTheme.Overlay100];
        }
    }
    
    public class Overlay2Extension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ThemeSystem.Instance
                              .Theme
                              .Colors[(int)ForestTheme.Overlay200];
        }
    }
    
    public class Overlay2BrushExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ThemeSystem.Instance
                              .Theme
                              .Colors[(int)ForestTheme.Overlay200]
                              .ToSolidColorBrush();
        }
    }
}