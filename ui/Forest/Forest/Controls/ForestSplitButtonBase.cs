using Acorisoft.FutureGL.Forest.Controls.Selectors;

namespace Acorisoft.FutureGL.Forest.Controls
{
    public abstract class ForestSplitButtonBase : ForestListBoxBase
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            nameof(Command),
            typeof(ICommand),
            typeof(ForestSplitButtonBase),
            new PropertyMetadata(default(ICommand)));
        //
        //
        // public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
        //     nameof(CommandParameter),
        //     typeof(object),
        //     typeof(ForestSplitButtonBase),
        //     new PropertyMetadata(default(object)));
        //
        //
        // public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register(
        //     nameof(CommandTarget),
        //     typeof(IInputElement),
        //     typeof(ForestSplitButtonBase),
        //     new PropertyMetadata(default(IInputElement)));


        // /// <summary>
        // /// 实现 <see cref="Header"/> 属性的依赖属性。
        // /// </summary>
        // public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        //     nameof(Header),
        //     typeof(object),
        //     typeof(ForestSplitButtonBase),
        //     new PropertyMetadata(default(object)));
        //
        // /// <summary>
        // /// 实现 <see cref="HeaderTemplate"/> 属性的依赖属性。
        // /// </summary>
        // public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(
        //     nameof(HeaderTemplate),
        //     typeof(DataTemplate),
        //     typeof(ForestSplitButtonBase),
        //     new PropertyMetadata(default(DataTemplate)));
        //
        // /// <summary>
        // /// 实现 <see cref="HeaderTemplateSelector"/> 属性的依赖属性。
        // /// </summary>
        // public static readonly DependencyProperty HeaderTemplateSelectorProperty = DependencyProperty.Register(
        //     nameof(HeaderTemplateSelector),
        //     typeof(DataTemplateSelector),
        //     typeof(ForestSplitButtonBase),
        //     new PropertyMetadata(default(DataTemplateSelector)));
        //
        // /// <summary>
        // /// 实现 <see cref="HeaderStringFormat"/> 属性的依赖属性。
        // /// </summary>
        // public static readonly DependencyProperty HeaderStringFormatProperty = DependencyProperty.Register(
        //     nameof(HeaderStringFormat),
        //     typeof(string),
        //     typeof(ForestSplitButtonBase),
        //     new PropertyMetadata(default(string)));

        public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(
            nameof(IsDropDownOpen),
            typeof(bool),
            typeof(ForestSplitButtonBase),
            new PropertyMetadata(Boxing.False));


        public static readonly DependencyProperty SelectionProperty = DependencyProperty.Register(
            nameof(Selection),
            typeof(object),
            typeof(ForestSplitButtonBase),
            new PropertyMetadata(default(object)));

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            IsDropDownOpen = false;
            base.OnLostFocus(e);
        }

        protected sealed override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            var originalItem = SelectedItem;
            
            
            if (originalItem is ContentControl contentControl)
            {
                Selection = contentControl.Content;
            }
            else if (originalItem is FrameworkElement fe)
            {
                Selection = fe.DataContext ?? fe.ToString();
            }
            else
            {
                Selection = originalItem;
            }
            
            //
            // Close Popup
            IsDropDownOpen = false;
            base.OnSelectionChanged(e);
            OnSelectionChangedOverride(e);
        }

        protected virtual void OnSelectionChangedOverride(SelectionChangedEventArgs e)
        {
            
        }

        public object Selection
        {
            get => (object)GetValue(SelectionProperty);
            set => SetValue(SelectionProperty, value);
        }

        public bool IsDropDownOpen
        {
            get => (bool)GetValue(IsDropDownOpenProperty);
            set => SetValue(IsDropDownOpenProperty, Boxing.Box(value));
        }
        
        // /// <summary>
        // /// 获取或设置 <see cref="HeaderStringFormat"/> 属性。
        // /// </summary>
        // public string HeaderStringFormat
        // {
        //     get => (string)GetValue(HeaderStringFormatProperty);
        //     set => SetValue(HeaderStringFormatProperty, value);
        // }
        //
        // /// <summary>
        // /// 获取或设置 <see cref="HeaderTemplateSelector"/> 属性。
        // /// </summary>
        // public DataTemplateSelector HeaderTemplateSelector
        // {
        //     get => (DataTemplateSelector)GetValue(HeaderTemplateSelectorProperty);
        //     set => SetValue(HeaderTemplateSelectorProperty, value);
        // }
        //
        // /// <summary>
        // /// 获取或设置 <see cref="HeaderTemplate"/> 属性。
        // /// </summary>
        // public DataTemplate HeaderTemplate
        // {
        //     get => (DataTemplate)GetValue(HeaderTemplateProperty);
        //     set => SetValue(HeaderTemplateProperty, value);
        // }
        //
        // /// <summary>
        // /// 获取或设置 <see cref="Header"/> 属性。
        // /// </summary>
        // public object Header
        // {
        //     get => (object)GetValue(HeaderProperty);
        //     set => SetValue(HeaderProperty, value);
        // }   

        // public IInputElement CommandTarget
        // {
        //     get => (IInputElement)GetValue(CommandTargetProperty);
        //     set => SetValue(CommandTargetProperty, value);
        // }
        //
        // public object CommandParameter
        // {
        //     get => (object)GetValue(CommandParameterProperty);
        //     set => SetValue(CommandParameterProperty, value);
        // }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }
    }
}