using System.Threading.Tasks;
using System.Windows.Input;

namespace Acorisoft.FutureGL.Forest.Controls
{
    public class Drawer : ContentControl
    {
        static Drawer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Drawer), new FrameworkPropertyMetadata(typeof(Drawer)));
        }

        public static readonly DependencyProperty IsLeftOpenProperty = DependencyProperty.Register(
            nameof(IsLeftOpen),
            typeof(bool),
            typeof(Drawer),
            new PropertyMetadata(Boxing.False, OnDrawerOpenStateChanged));

        public static readonly DependencyProperty LeftContentProperty = DependencyProperty.Register(
            nameof(LeftContent),
            typeof(object),
            typeof(Drawer),
            new PropertyMetadata(default(object)));

        public static readonly DependencyProperty LeftContentTemplateProperty = DependencyProperty.Register(
            nameof(LeftContentTemplate),
            typeof(DataTemplate),
            typeof(Drawer),
            new PropertyMetadata(default(DataTemplate)));

        public static readonly DependencyProperty LeftContentTemplateSelectorProperty = DependencyProperty.Register(
            nameof(LeftContentTemplateSelector),
            typeof(DataTemplateSelector),
            typeof(Drawer),
            new PropertyMetadata(default(DataTemplateSelector)));

        public static readonly DependencyProperty LeftContentStringFormatProperty = DependencyProperty.Register(
            nameof(LeftContentStringFormat),
            typeof(string),
            typeof(Drawer),
            new PropertyMetadata(default(string)));


        public static readonly DependencyProperty IsRightOpenProperty = DependencyProperty.Register(
            nameof(IsRightOpen),
            typeof(bool),
            typeof(Drawer),
            new PropertyMetadata(Boxing.False, OnDrawerOpenStateChanged));

        public static readonly DependencyProperty RightContentProperty = DependencyProperty.Register(
            nameof(RightContent),
            typeof(object),
            typeof(Drawer),
            new PropertyMetadata(default(object)));

        public static readonly DependencyProperty RightContentTemplateProperty = DependencyProperty.Register(
            nameof(RightContentTemplate),
            typeof(DataTemplate),
            typeof(Drawer),
            new PropertyMetadata(default(DataTemplate)));

        public static readonly DependencyProperty RightContentTemplateSelectorProperty = DependencyProperty.Register(
            nameof(RightContentTemplateSelector),
            typeof(DataTemplateSelector),
            typeof(Drawer),
            new PropertyMetadata(default(DataTemplateSelector)));

        public static readonly DependencyProperty RightContentStringFormatProperty = DependencyProperty.Register(
            nameof(RightContentStringFormat),
            typeof(string),
            typeof(Drawer),
            new PropertyMetadata(default(string)));


        public static readonly DependencyProperty IsTopOpenProperty = DependencyProperty.Register(
            nameof(IsTopOpen),
            typeof(bool),
            typeof(Drawer),
            new PropertyMetadata(Boxing.False, OnDrawerOpenStateChanged));

        public static readonly DependencyProperty TopContentProperty = DependencyProperty.Register(
            nameof(TopContent),
            typeof(object),
            typeof(Drawer),
            new PropertyMetadata(default(object)));

        public static readonly DependencyProperty TopContentTemplateProperty = DependencyProperty.Register(
            nameof(TopContentTemplate),
            typeof(DataTemplate),
            typeof(Drawer),
            new PropertyMetadata(default(DataTemplate)));

        public static readonly DependencyProperty TopContentTemplateSelectorProperty = DependencyProperty.Register(
            nameof(TopContentTemplateSelector),
            typeof(DataTemplateSelector),
            typeof(Drawer),
            new PropertyMetadata(default(DataTemplateSelector)));

        public static readonly DependencyProperty TopContentStringFormatProperty = DependencyProperty.Register(
            nameof(TopContentStringFormat),
            typeof(string),
            typeof(Drawer),
            new PropertyMetadata(default(string)));


        public static readonly DependencyProperty IsBottomOpenProperty = DependencyProperty.Register(
            nameof(IsBottomOpen),
            typeof(bool),
            typeof(Drawer),
            new PropertyMetadata(Boxing.False, OnDrawerOpenStateChanged));

        public static readonly DependencyProperty BottomContentProperty = DependencyProperty.Register(
            nameof(BottomContent),
            typeof(object),
            typeof(Drawer),
            new PropertyMetadata(default(object)));

        public static readonly DependencyProperty BottomContentTemplateProperty = DependencyProperty.Register(
            nameof(BottomContentTemplate),
            typeof(DataTemplate),
            typeof(Drawer),
            new PropertyMetadata(default(DataTemplate)));

        public static readonly DependencyProperty BottomContentTemplateSelectorProperty = DependencyProperty.Register(
            nameof(BottomContentTemplateSelector),
            typeof(DataTemplateSelector),
            typeof(Drawer),
            new PropertyMetadata(default(DataTemplateSelector)));

        public static readonly DependencyProperty BottomContentStringFormatProperty = DependencyProperty.Register(
            nameof(BottomContentStringFormat),
            typeof(string),
            typeof(Drawer),
            new PropertyMetadata(default(string)));

        // private static void OnDrawerOpenStateChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        // {
        //     var drawer = (Drawer)element;
        //
        //     if(drawer.Part_mask is null)
        //     {
        //         return;
        //     }
        //
        //     drawer.Focus();
        //     drawer.Part_mask.Visibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        // }

        private Storyboard _storyboard;

        private void CreateMaskAnimation(bool close)
        {
            if (_partMask is null)
            {
                return;
            }
            
            _storyboard?.Stop(_partMask);
            
            if (close)
            {
                var opacityAnimation = new DoubleAnimation
                {
                    Duration = new Duration(TimeSpan.FromMilliseconds(350)),
                    From     = 1d,
                    To       = 0d,
                };

                Storyboard.SetTarget(opacityAnimation, _partMask);
                Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath("(UIElement.Opacity)"));

                _storyboard = new Storyboard
                {
                    Children = new TimelineCollection { opacityAnimation }
                };
                
                //
                // 开始动画
                _partMask.BeginStoryboard(_storyboard, HandoffBehavior.SnapshotAndReplace, true);
                
                Task.Run(async () =>
                {
                    await Task.Delay(350);
                    Dispatcher.Invoke(() =>
                    {
                        _partMask.Visibility = Visibility.Collapsed;
                    });
                });
            }
            else
            {
                
                _partMask.Visibility = Visibility.Visible;
                var opacityAnimation = new DoubleAnimation
                {
                    Duration = new Duration(TimeSpan.FromMilliseconds(500)),
                    From     = 0d,
                    To       = 1d,
                };

                Storyboard.SetTarget(opacityAnimation, _partMask);
                Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath("(UIElement.Opacity)"));

                _storyboard = new Storyboard
                {
                    Children = new TimelineCollection { opacityAnimation }
                };
                
                //
                // 开始动画
                _partMask.BeginStoryboard(_storyboard, HandoffBehavior.SnapshotAndReplace, true);
            }
        }

        private static void OnDrawerOpenStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is bool b && b)
            {
                ((Drawer)d).CreateMaskAnimation(false);
            }
            else
            {
                ((Drawer)d).CreateMaskAnimation(true);
            }
        }

        private Border _partMask;

        public override void OnApplyTemplate()
        {
            _partMask = GetTemplateChild("mask") as Border;

            if (_partMask is not null)
            {
                _partMask.MouseLeftButtonDown += ResetDrawerOpenState;
            }

            if (IsLeftOpen ||
                IsRightOpen ||
                IsTopOpen ||
                IsBottomOpen)
            {
                CreateMaskAnimation(false);
            }
        }
        
        public void Close() 
        {
            IsLeftOpen = 
                IsRightOpen = 
                    IsBottomOpen = 
                        IsTopOpen = false;
            
            RaiseEvent(new RoutedEventArgs
            {
                RoutedEvent = DrawerClosedEvent
            });
        }

        private void ResetDrawerOpenState(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        public string BottomContentStringFormat
        {
            get => (string)GetValue(BottomContentStringFormatProperty);
            set => SetValue(BottomContentStringFormatProperty, value);
        }

        public DataTemplateSelector BottomContentTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(BottomContentTemplateSelectorProperty);
            set => SetValue(BottomContentTemplateSelectorProperty, value);
        }

        public DataTemplate BottomContentTemplate
        {
            get => (DataTemplate)GetValue(BottomContentTemplateProperty);
            set => SetValue(BottomContentTemplateProperty, value);
        }

        public object BottomContent
        {
            get => GetValue(BottomContentProperty);
            set => SetValue(BottomContentProperty, value);
        }

        public bool IsBottomOpen
        {
            get => (bool)GetValue(IsBottomOpenProperty);
            set => SetValue(IsBottomOpenProperty, Boxing.Box(value));
        }

        public string TopContentStringFormat
        {
            get => (string)GetValue(TopContentStringFormatProperty);
            set => SetValue(TopContentStringFormatProperty, value);
        }

        public DataTemplateSelector TopContentTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(TopContentTemplateSelectorProperty);
            set => SetValue(TopContentTemplateSelectorProperty, value);
        }

        public DataTemplate TopContentTemplate
        {
            get => (DataTemplate)GetValue(TopContentTemplateProperty);
            set => SetValue(TopContentTemplateProperty, value);
        }

        public object TopContent
        {
            get => GetValue(TopContentProperty);
            set => SetValue(TopContentProperty, value);
        }

        public bool IsTopOpen
        {
            get => (bool)GetValue(IsTopOpenProperty);
            set => SetValue(IsTopOpenProperty, Boxing.Box(value));
        }

        public string RightContentStringFormat
        {
            get => (string)GetValue(RightContentStringFormatProperty);
            set => SetValue(RightContentStringFormatProperty, value);
        }

        public DataTemplateSelector RightContentTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(RightContentTemplateSelectorProperty);
            set => SetValue(RightContentTemplateSelectorProperty, value);
        }

        public DataTemplate RightContentTemplate
        {
            get => (DataTemplate)GetValue(RightContentTemplateProperty);
            set => SetValue(RightContentTemplateProperty, value);
        }

        public object RightContent
        {
            get => GetValue(RightContentProperty);
            set => SetValue(RightContentProperty, value);
        }

        public bool IsRightOpen
        {
            get => (bool)GetValue(IsRightOpenProperty);
            set => SetValue(IsRightOpenProperty, Boxing.Box(value));
        }

        public string LeftContentStringFormat
        {
            get => (string)GetValue(LeftContentStringFormatProperty);
            set => SetValue(LeftContentStringFormatProperty, value);
        }

        public DataTemplateSelector LeftContentTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(LeftContentTemplateSelectorProperty);
            set => SetValue(LeftContentTemplateSelectorProperty, value);
        }

        public DataTemplate LeftContentTemplate
        {
            get => (DataTemplate)GetValue(LeftContentTemplateProperty);
            set => SetValue(LeftContentTemplateProperty, value);
        }

        public object LeftContent
        {
            get => GetValue(LeftContentProperty);
            set => SetValue(LeftContentProperty, value);
        }

        public bool IsLeftOpen
        {
            get => (bool)GetValue(IsLeftOpenProperty);
            set => SetValue(IsLeftOpenProperty, Boxing.Box(value));
        }

        public static readonly RoutedEvent DrawerClosedEvent =
            EventManager.RegisterRoutedEvent(
                nameof(DrawerClosedEvent),
                RoutingStrategy.Bubble, 
                typeof(RoutedEventHandler), 
                typeof(Drawer));
        
        public event RoutedEventHandler DrawerClosed
        {
            add => AddHandler(DrawerClosedEvent, value);
            remove => RemoveHandler(DrawerClosedEvent, value);
        }
    }
}