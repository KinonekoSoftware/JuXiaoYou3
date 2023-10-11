using Acorisoft.FutureGL.MigaStudio.Models;

namespace Acorisoft.FutureGL.MigaStudio.Resources.Converters
{
    public class SettingItemTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return item switch
            {
                ISettingComboBox => Resources["ComboBox"] as DataTemplate,
                SettingSlider    => Resources["Slider"] as DataTemplate,
                _                => base.SelectTemplate(item, container),
            };
        }

        public ResourceDictionary Resources { get; set; }
    }
}