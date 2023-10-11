namespace Acorisoft.FutureGL.Forest.UI
{
    public static class ForestUI
    {
        public static ResourceDictionary UseToolKits()
        {
            return new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/Forest.Controls;component/UI/Tools/Themes.xaml", UriKind.RelativeOrAbsolute)
            };
        }
    }
}