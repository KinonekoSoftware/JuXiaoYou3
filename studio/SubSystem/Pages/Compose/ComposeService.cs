using System.Windows;
using System.Windows.Media;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Composes
{
    public class ComposeService : ObservableObject
    {
        private Geometry         _icon;
        private string           _serviceName;
        private FrameworkElement _view;
        private Compose          _compose;
        private ComposeCache     _cache;
        private ComposeEditorBase           _viewModel;

        /// <summary>
        /// 获取或设置 <see cref="ViewModel"/> 属性。
        /// </summary>
        public ComposeEditorBase ViewModel
        {
            get => _viewModel;
            set => SetValue(ref _viewModel, value);
        }
        /// <summary>
        /// 获取或设置 <see cref="Cache"/> 属性。
        /// </summary>
        public ComposeCache Cache
        {
            get => _cache;
            set => SetValue(ref _cache, value);
        }
        /// <summary>
        /// 获取或设置 <see cref="Compose"/> 属性。
        /// </summary>
        public Compose Compose
        {
            get => _compose;
            set => SetValue(ref _compose, value);
        }
        /// <summary>
        /// 获取或设置 <see cref="View"/> 属性。
        /// </summary>
        public FrameworkElement View
        {
            get => _view;
            set => SetValue(ref _view, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="ServiceName"/> 属性。
        /// </summary>
        public string ServiceName
        {
            get => _serviceName;
            set => SetValue(ref _serviceName, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Icon"/> 属性。
        /// </summary>
        public Geometry Icon
        {
            get => _icon;
            set => SetValue(ref _icon, value);
        }
    }
}