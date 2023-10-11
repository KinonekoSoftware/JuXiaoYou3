using Acorisoft.FutureGL.Forest.Controls.Buttons;

namespace Acorisoft.FutureGL.MigaStudio.Controls
{
    public class QuickStartPane : LooklessButton
    {
        static QuickStartPane()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(QuickStartPane), new FrameworkPropertyMetadata(typeof(QuickStartPane)));
        }
        
        public static readonly DependencyProperty AccentProperty = DependencyProperty.Register(
            nameof(Accent),
            typeof(Brush),
            typeof(QuickStartPane),
            new PropertyMetadata(default(Brush)));

        /// <summary>
        /// 实现 <see cref="Header"/> 属性的依赖属性。
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            nameof(Header),
            typeof(object),
            typeof(QuickStartPane),
            new PropertyMetadata(default(object)));

        /// <summary>
        /// 实现 <see cref="HeaderTemplate"/> 属性的依赖属性。
        /// </summary>
        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(
            nameof(HeaderTemplate),
            typeof(DataTemplate),
            typeof(QuickStartPane),
            new PropertyMetadata(default(DataTemplate)));

        /// <summary>
        /// 实现 <see cref="HeaderTemplateSelector"/> 属性的依赖属性。
        /// </summary>
        public static readonly DependencyProperty HeaderTemplateSelectorProperty = DependencyProperty.Register(
            nameof(HeaderTemplateSelector),
            typeof(DataTemplateSelector),
            typeof(QuickStartPane),
            new PropertyMetadata(default(DataTemplateSelector)));

        /// <summary>
        /// 实现 <see cref="HeaderStringFormat"/> 属性的依赖属性。
        /// </summary>
        public static readonly DependencyProperty HeaderStringFormatProperty = DependencyProperty.Register(
            nameof(HeaderStringFormat),
            typeof(string),
            typeof(QuickStartPane),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty HeaderBrushProperty = DependencyProperty.Register(
            nameof(HeaderBrush),
            typeof(Brush),
            typeof(QuickStartPane),
            new PropertyMetadata(default(Brush)));

        public Brush HeaderBrush
        {
            get => (Brush)GetValue(HeaderBrushProperty);
            set => SetValue(HeaderBrushProperty, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="HeaderStringFormat"/> 属性。
        /// </summary>
        public string HeaderStringFormat
        {
            get => (string)GetValue(HeaderStringFormatProperty);
            set => SetValue(HeaderStringFormatProperty, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="HeaderTemplateSelector"/> 属性。
        /// </summary>
        public DataTemplateSelector HeaderTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(HeaderTemplateSelectorProperty);
            set => SetValue(HeaderTemplateSelectorProperty, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="HeaderTemplate"/> 属性。
        /// </summary>
        public DataTemplate HeaderTemplate
        {
            get => (DataTemplate)GetValue(HeaderTemplateProperty);
            set => SetValue(HeaderTemplateProperty, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Header"/> 属性。
        /// </summary>
        public object Header
        {
            get => (object)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }   

        public Brush Accent
        {
            get => (Brush)GetValue(AccentProperty);
            set => SetValue(AccentProperty, value);
        }
    }
}