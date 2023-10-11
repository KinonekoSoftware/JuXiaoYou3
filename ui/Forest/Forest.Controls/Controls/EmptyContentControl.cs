using System.ComponentModel;
using System.Windows.Markup;

namespace Acorisoft.FutureGL.Forest.Controls
{
    [DefaultProperty("Content")]
    [ContentProperty("Content")]
    public class EmptyContentControl : ContentControl
    {
        public static readonly DependencyProperty    EmptyStateProperty;
        public static readonly DependencyProperty    EmptyStateTemplateProperty;
        public static readonly DependencyProperty    EmptyStateTemplateSelectorProperty;
        public static readonly DependencyProperty    EmptyStateStringFormatProperty;
        public static readonly DependencyProperty    EmptyStateBrushProperty;
        public static readonly DependencyProperty    IsEmptyProperty ;
        public static readonly DependencyProperty    NotEmptyProperty;
        public static readonly DependencyPropertyKey ShowEmptyStatePropertyKey;
        public static readonly DependencyProperty    ShowEmptyStateProperty;
        
        static EmptyContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EmptyContentControl), new FrameworkPropertyMetadata(typeof(EmptyContentControl)));
            IsEmptyProperty = DependencyProperty.Register(
                nameof(IsEmpty),
                typeof(bool),
                typeof(EmptyContentControl),
                new PropertyMetadata(Boxing.True, OnIsEmptyChanged));
            NotEmptyProperty = DependencyProperty.Register(
                nameof(NotEmpty),
                typeof(bool),
                typeof(EmptyContentControl),
                new PropertyMetadata(Boxing.False, OnNotEmptyChanged));
            ShowEmptyStatePropertyKey = DependencyProperty.RegisterReadOnly(
                nameof(ShowEmptyState),
                typeof(bool),
                typeof(EmptyContentControl),
                new PropertyMetadata(Boxing.True));
            
            EmptyStateProperty = DependencyProperty.Register(
                nameof(EmptyState),
                typeof(object),
                typeof(EmptyContentControl),
                new PropertyMetadata(default(object)));
            
            EmptyStateTemplateProperty = DependencyProperty.Register(
                nameof(EmptyStateTemplate),
                typeof(DataTemplate),
                typeof(EmptyContentControl),
                new PropertyMetadata(default(DataTemplate)));
            EmptyStateTemplateSelectorProperty = DependencyProperty.Register(
                nameof(EmptyStateTemplateSelector),
                typeof(DataTemplateSelector),
                typeof(EmptyContentControl),
                new PropertyMetadata(default(DataTemplateSelector)));
            EmptyStateStringFormatProperty = DependencyProperty.Register(
                nameof(EmptyStateStringFormat),
                typeof(string),
                typeof(EmptyContentControl),
                new PropertyMetadata(default(string)));
            EmptyStateBrushProperty = DependencyProperty.Register(
                nameof(EmptyStateBrush),
                typeof(Brush),
                typeof(EmptyContentControl),
                new PropertyMetadata(default(Brush)));
            ShowEmptyStateProperty = ShowEmptyStatePropertyKey.DependencyProperty;
        }

        private static void OnIsEmptyChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            element.SetValue(ShowEmptyStatePropertyKey, e.NewValue);
        }
        
        private static void OnNotEmptyChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            element.SetValue(ShowEmptyStatePropertyKey, Boxing.Box(!((bool)e.NewValue)));
        }

        public bool ShowEmptyState
        {
            get => (bool)GetValue(ShowEmptyStateProperty);
            private set => SetValue(ShowEmptyStateProperty, value);
        }

        public bool NotEmpty
        {
            get => (bool)GetValue(NotEmptyProperty);
            set => SetValue(NotEmptyProperty,  Boxing.Box(value));
        }

        public bool IsEmpty
        {
            get => (bool)GetValue(IsEmptyProperty);
            set => SetValue(IsEmptyProperty, Boxing.Box(value));
        }


        public Brush EmptyStateBrush
        {
            get => (Brush)GetValue(EmptyStateBrushProperty);
            set => SetValue(EmptyStateBrushProperty, value);
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