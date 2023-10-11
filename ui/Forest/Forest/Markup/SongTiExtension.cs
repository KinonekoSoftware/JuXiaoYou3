using System.Windows.Markup;

namespace Acorisoft.FutureGL.Forest.Markup
{
    public class SongTiExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Application.Current.Resources["SongTi"] as FontFamily;
        }
    }
    
    public class HeiTiExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Application.Current.Resources["HeiTi"] as FontFamily;
        }
    }
    
    public class ShouXieTiExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Application.Current.Resources["ShouXieTi"] as FontFamily;
        }
    }
}