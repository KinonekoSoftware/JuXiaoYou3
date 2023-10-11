using System.Collections.Generic;
using System.Collections.ObjectModel;
using Acorisoft.FutureGL.MigaStudio.Models;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public abstract class SettingViewModelBase : TabViewModel
    {
        protected SettingViewModelBase()
        {
            Items = new ObservableCollection<ISettingItem>();
        }


        protected ISettingItem Text(string id, string defaultValue, Action<string> callback)
        {
            var mainTitleKey = $"{id}";
            var subTitleKey  = $"{id}.Tips";

            var item = new SettingField
            {
                MainTitle = Language.GetText(mainTitleKey),
                SubTitle  = Language.GetText(subTitleKey),
                Callback  = callback,
                Value     = defaultValue
            };

            Items.Add(item);
            return item;
        }

        protected ISettingItem ComboBox<T>(string id, T defaultValue, Action<T> callback,
            IEnumerable<object> collection)
        {
            var mainTitleKey = $"{id}";
            var subTitleKey  = $"{id}.Tips";

            var item = new SettingComboBox<T>
            {
                MainTitle  = Language.GetText(mainTitleKey),
                SubTitle   = Language.GetText(subTitleKey),
                Callback   = callback,
                Collection = collection,
                Value      = defaultValue
            };

            Items.Add(item);
            return item;
        }


        protected ISettingItem Slider(string id, int defaultValue, Action<int> callback, int min, int max)
        {
            var mainTitleKey = $"{id}";
            var subTitleKey  = $"{id}.Tips";
            var unitKey = $"{id}.Unit";
            defaultValue = Math.Clamp(defaultValue, min, max);

            var item = new SettingSlider
            {
                MainTitle = Language.GetText(mainTitleKey),
                SubTitle  = Language.GetText(subTitleKey),
                Unit      =  Language.GetText(unitKey),
                Callback  = callback,
                Maximum   = max,
                Minimum   = min,
                Value     = defaultValue
            };

            Items.Add(item);
            return item;
        }

        public ObservableCollection<ISettingItem> Items { get; }
    }
}