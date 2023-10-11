using System.Windows.Controls;
// ReSharper disable MemberCanBeMadeStatic.Local

namespace Acorisoft.FutureGL.Forest.Controls
{
    public abstract class ViewHostBase : Control
    {
        #region Static Methods

        static ViewHostBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ViewHostBase), new FrameworkPropertyMetadata(typeof(ViewHostBase)));
            ViewModelProperty = DependencyProperty.Register(
                "ViewModel",
                typeof(ViewModelBase),
                typeof(ViewHostBase),
                new PropertyMetadata(default(ViewModelBase), OnViewModelChanged));

            ContentPropertyKey = DependencyProperty.RegisterReadOnly(
                "Content",
                typeof(object),
                typeof(ViewHostBase),
                new PropertyMetadata(default(object)));
            ContentProperty = ContentPropertyKey.DependencyProperty;
        }

        public static readonly DependencyProperty    ViewModelProperty;
        public static readonly DependencyPropertyKey ContentPropertyKey;
        public static readonly DependencyProperty    ContentProperty;

        internal static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var host = (ViewHostBase)d;

            if (e.OldValue is ViewModelBase oldPage)
            {
                oldPage.Suspend();
            }

            if (e.NewValue is ViewModelBase newViewModel)
            {
                host.OnViewModelChanged(newViewModel);
            }
            else
            {
                d.ClearValue(ContentPropertyKey);
            }
        }

        #endregion

        private ViewModelBase _current;

        protected virtual void OnViewModelChanged(ViewModelBase vm)
        {
            _current ??= vm;

            //
            // 获得页面
            var page = (FrameworkElement)Xaml.Connect(vm);

            //
            //
            if (page is null)
            {
                return;
            }

            //
            // 设置数据上下文
            page.DataContext =  vm;

            if (page is not ForestUserControl)
            {
                page.Loaded   += ViewModelStarting;
                page.Unloaded += ViewModelStopping;
            }

            //
            // 设置内容
            Content = page;
        }

        private void ViewModelStopping(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement { DataContext: ViewModelBase vm } fe)
            {
                fe.Unloaded -= ViewModelStopping;
                vm.Suspend();
            }
        }

        private void ViewModelStarting(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement { DataContext: ViewModelBase vm } fe)
            {
                //
                // 注意：在第一次执行Loaded之后就要取消事件订阅
                fe.Loaded -= ViewModelStarting;
                vm.Start();
            }
        }

        /// <summary>
        /// 内容
        /// </summary>
        public object Content
        {
            get => GetValue(ContentProperty);
            protected set => SetValue(ContentPropertyKey, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public object ViewModel
        {
            get => (object)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }
    }
}