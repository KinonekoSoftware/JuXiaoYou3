using System.Windows.Controls.Primitives;

namespace Acorisoft.FutureGL.MigaStudio.Controls
{
    public class BinaryControl : ToggleButton
    {
        static BinaryControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BinaryControl),
                new FrameworkPropertyMetadata(typeof(BinaryControl)));
        }

        /// <summary>
        /// 实现 <see cref="Negative"/> 属性的依赖属性。
        /// </summary>
        public static readonly DependencyProperty NegativeProperty = DependencyProperty.Register(
            nameof(Negative),
            typeof(object),
            typeof(BinaryControl),
            new PropertyMetadata(default(object)));

        /// <summary>
        /// 实现 <see cref="NegativeTemplate"/> 属性的依赖属性。
        /// </summary>
        public static readonly DependencyProperty NegativeTemplateProperty = DependencyProperty.Register(
            nameof(NegativeTemplate),
            typeof(DataTemplate),
            typeof(BinaryControl),
            new PropertyMetadata(default(DataTemplate)));

        /// <summary>
        /// 实现 <see cref="NegativeTemplateSelector"/> 属性的依赖属性。
        /// </summary>
        public static readonly DependencyProperty NegativeTemplateSelectorProperty = DependencyProperty.Register(
            nameof(NegativeTemplateSelector),
            typeof(DataTemplateSelector),
            typeof(BinaryControl),
            new PropertyMetadata(default(DataTemplateSelector)));

        /// <summary>
        /// 实现 <see cref="NegativeStringFormat"/> 属性的依赖属性。
        /// </summary>
        public static readonly DependencyProperty NegativeStringFormatProperty = DependencyProperty.Register(
            nameof(NegativeStringFormat),
            typeof(string),
            typeof(BinaryControl),
            new PropertyMetadata(default(string)));

        /// <summary>
        /// 实现 <see cref="Positive"/> 属性的依赖属性。
        /// </summary>
        public static readonly DependencyProperty PositiveProperty = DependencyProperty.Register(
            nameof(Positive),
            typeof(object),
            typeof(BinaryControl),
            new PropertyMetadata(default(object)));

        /// <summary>
        /// 实现 <see cref="PositiveTemplate"/> 属性的依赖属性。
        /// </summary>
        public static readonly DependencyProperty PositiveTemplateProperty = DependencyProperty.Register(
            nameof(PositiveTemplate),
            typeof(DataTemplate),
            typeof(BinaryControl),
            new PropertyMetadata(default(DataTemplate)));

        /// <summary>
        /// 实现 <see cref="PositiveTemplateSelector"/> 属性的依赖属性。
        /// </summary>
        public static readonly DependencyProperty PositiveTemplateSelectorProperty = DependencyProperty.Register(
            nameof(PositiveTemplateSelector),
            typeof(DataTemplateSelector),
            typeof(BinaryControl),
            new PropertyMetadata(default(DataTemplateSelector)));

        /// <summary>
        /// 实现 <see cref="PositiveStringFormat"/> 属性的依赖属性。
        /// </summary>
        public static readonly DependencyProperty PositiveStringFormatProperty = DependencyProperty.Register(
            nameof(PositiveStringFormat),
            typeof(string),
            typeof(BinaryControl),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            nameof(Header),
            typeof(string),
            typeof(BinaryControl),
            new PropertyMetadata(default(string)));



        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(string),
            typeof(BinaryControl), 
            new PropertyMetadata(null, OnValueChanged));

        private bool _valueChanged;
        private static void OnValueChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            var ov = (BinaryControl)element;
            var v = e.NewValue?.ToString();
            if (ov._valueChanged)
            {
                ov._valueChanged = false;
                return;
            }
            
            ov.IsChecked = v == ov.Positive?.ToString();

        }

        protected override void OnClick()
        {
            var check = IsChecked == true;
            
            Value = check ? Positive?.ToString() : Negative?.ToString();

            base.OnClick();
        }

        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="PositiveStringFormat"/> 属性。
        /// </summary>
        public string PositiveStringFormat
        {
            get => (string)GetValue(PositiveStringFormatProperty);
            set => SetValue(PositiveStringFormatProperty, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="PositiveTemplateSelector"/> 属性。
        /// </summary>
        public DataTemplateSelector PositiveTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(PositiveTemplateSelectorProperty);
            set => SetValue(PositiveTemplateSelectorProperty, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="PositiveTemplate"/> 属性。
        /// </summary>
        public DataTemplate PositiveTemplate
        {
            get => (DataTemplate)GetValue(PositiveTemplateProperty);
            set => SetValue(PositiveTemplateProperty, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Positive"/> 属性。
        /// </summary>
        public object Positive
        {
            get => GetValue(PositiveProperty);
            set => SetValue(PositiveProperty, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="NegativeStringFormat"/> 属性。
        /// </summary>
        public string NegativeStringFormat
        {
            get => (string)GetValue(NegativeStringFormatProperty);
            set => SetValue(NegativeStringFormatProperty, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="NegativeTemplateSelector"/> 属性。
        /// </summary>
        public DataTemplateSelector NegativeTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(NegativeTemplateSelectorProperty);
            set => SetValue(NegativeTemplateSelectorProperty, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="NegativeTemplate"/> 属性。
        /// </summary>
        public DataTemplate NegativeTemplate
        {
            get => (DataTemplate)GetValue(NegativeTemplateProperty);
            set => SetValue(NegativeTemplateProperty, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Negative"/> 属性。
        /// </summary>
        public object Negative
        {
            get => GetValue(NegativeProperty);
            set => SetValue(NegativeProperty, value);
        }
    }
}