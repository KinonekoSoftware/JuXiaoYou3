using System.Windows.Markup;

namespace Acorisoft.FutureGL.Forest.Markup
{
    public class ColorExtension : MarkupExtension
    {
        public ColorExtension(){}
        public ColorExtension(ForestTheme theme) => Theme = theme;
        
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ThemeSystem.Instance.Theme.Colors[(int)Theme];
        }
        
        public ForestTheme Theme { get; set; }
    }
}