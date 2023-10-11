using System.Windows.Controls;

// ReSharper disable PropertyCanBeMadeInitOnly.Local

namespace Acorisoft.FutureGL.Forest.Controls
{
    public class EmptyContentHost : ViewHostBase
    {
        public static readonly DependencyProperty    EmptyStateProperty;
        public static readonly DependencyProperty    EmptyStateTemplateProperty;
        public static readonly DependencyProperty    EmptyStateTemplateSelectorProperty;
        public static readonly DependencyProperty    EmptyStateStringFormatProperty;
        public static readonly DependencyPropertyKey IsEmptyStatePropertyKey;
        public static readonly DependencyProperty    IsEmptyStateProperty;

        static EmptyContentHost()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EmptyContentHost), new FrameworkPropertyMetadata(typeof(EmptyContentHost)));
            IsEmptyStatePropertyKey = DependencyProperty.RegisterReadOnly(
                nameof(IsEmptyState),
                typeof(bool),
                typeof(EmptyContentHost),
                new PropertyMetadata(Boxing.False));

            EmptyStateProperty = DependencyProperty.Register(
                nameof(EmptyState),
                typeof(object),
                typeof(EmptyContentHost),
                new PropertyMetadata(default(object)));
            EmptyStateTemplateProperty = DependencyProperty.Register(
                nameof(EmptyStateTemplate),
                typeof(DataTemplate),
                typeof(EmptyContentHost),
                new PropertyMetadata(default(DataTemplate)));
            EmptyStateTemplateSelectorProperty = DependencyProperty.Register(
                nameof(EmptyStateTemplateSelector),
                typeof(DataTemplateSelector),
                typeof(EmptyContentHost),
                new PropertyMetadata(default(DataTemplateSelector)));
            EmptyStateStringFormatProperty = DependencyProperty.Register(
                nameof(EmptyStateStringFormat),
                typeof(string),
                typeof(EmptyContentHost),
                new PropertyMetadata(default(string)));

            IsEmptyStateProperty = IsEmptyStatePropertyKey.DependencyProperty;
        }

        protected override void OnViewModelChanged(ViewModelBase vm)
        {
            IsEmptyState = ViewModel is null && vm is null;
            base.OnViewModelChanged(vm);
        }

        public bool IsEmptyState
        {
            get => (bool)GetValue(IsEmptyStateProperty);
            private set => SetValue(IsEmptyStatePropertyKey, Boxing.Box(value));
        }

        /// <summary>
        /// 获取或设置 <see cref="EmptyStateStringFormat"/> 属性。
        /// </summary>
        public string EmptyStateStringFormat
        {
            get => (string)GetValue(EmptyStateStringFormatProperty);
            set => SetValue(EmptyStateStringFormatProperty, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="EmptyStateTemplateSelector"/> 属性。
        /// </summary>
        public DataTemplateSelector EmptyStateTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(EmptyStateTemplateSelectorProperty);
            set => SetValue(EmptyStateTemplateSelectorProperty, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="EmptyStateTemplate"/> 属性。
        /// </summary>
        public DataTemplate EmptyStateTemplate
        {
            get => (DataTemplate)GetValue(EmptyStateTemplateProperty);
            set => SetValue(EmptyStateTemplateProperty, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="EmptyState"/> 属性。
        /// </summary>
        public object EmptyState
        {
            get => (object)GetValue(EmptyStateProperty);
            set => SetValue(EmptyStateProperty, value);
        }
    }
}