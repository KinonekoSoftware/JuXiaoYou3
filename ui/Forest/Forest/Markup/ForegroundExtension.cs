using System.Windows.Markup;

namespace Acorisoft.FutureGL.Forest.Markup
{
    public class ForegroundExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ThemeSystem.Instance.Theme.Colors[(int)ForestTheme.ForegroundLevel1];
        }
    }

    public class ForegroundBrushExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ThemeSystem.Instance.Theme.Colors[(int)ForestTheme.ForegroundLevel1].ToSolidColorBrush();
        }
    }


    public class BackgroundExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ThemeSystem.Instance.Theme.Colors[(int)ForestTheme.BackgroundLevel3];
        }
    }

    public class BackgroundBrushExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ThemeSystem.Instance.Theme.Colors[(int)ForestTheme.BackgroundLevel3].ToSolidColorBrush();
        }
    }
    
    public class DisabledExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ThemeSystem.Instance.Theme.Colors[(int)ForestTheme.BackgroundDisabled];
        }
    }

    public class DisabledBrushExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ThemeSystem.Instance.Theme.Colors[(int)ForestTheme.BackgroundDisabled].ToSolidColorBrush();
        }
    }
}