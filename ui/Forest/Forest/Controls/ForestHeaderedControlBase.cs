namespace Acorisoft.FutureGL.Forest.Controls
{
    public abstract class ForestHeaderedControlBase : ForestContentControlBase, ITextResourceAdapter
    {
        /// <summary>
        /// 实现 <see cref="Header"/> 属性的依赖属性。
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            nameof(Header),
            typeof(object),
            typeof(ForestHeaderedControlBase),
            new PropertyMetadata(default(object)));

        /// <summary>
        /// 实现 <see cref="HeaderTemplate"/> 属性的依赖属性。
        /// </summary>
        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(
            nameof(HeaderTemplate),
            typeof(DataTemplate),
            typeof(ForestHeaderedControlBase),
            new PropertyMetadata(default(DataTemplate)));

        /// <summary>
        /// 实现 <see cref="HeaderTemplateSelector"/> 属性的依赖属性。
        /// </summary>
        public static readonly DependencyProperty HeaderTemplateSelectorProperty = DependencyProperty.Register(
            nameof(HeaderTemplateSelector),
            typeof(DataTemplateSelector),
            typeof(ForestHeaderedControlBase),
            new PropertyMetadata(default(DataTemplateSelector)));

        /// <summary>
        /// 实现 <see cref="HeaderStringFormat"/> 属性的依赖属性。
        /// </summary>
        public static readonly DependencyProperty HeaderStringFormatProperty = DependencyProperty.Register(
            nameof(HeaderStringFormat),
            typeof(string),
            typeof(ForestHeaderedControlBase),
            new PropertyMetadata(default(string)));
        
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
        void ITextResourceAdapter.SetText(string text)
        {
            Header = text;
        }

        void ITextResourceAdapter.SetToolTips(string text)
        {
            ToolTip = text;
        }
    }
}