using System.Windows.Markup;

namespace Acorisoft.FutureGL.Forest.Markup
{
    public class BrushExtension : MarkupExtension
    {
        public BrushExtension(){}
        public BrushExtension(ForestTheme theme) => Theme = theme;
        
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ThemeSystem.Instance.Theme.Colors[(int)Theme].ToSolidColorBrush();
        }
        
        
        public ForestTheme Theme { get; set; }
    }
}