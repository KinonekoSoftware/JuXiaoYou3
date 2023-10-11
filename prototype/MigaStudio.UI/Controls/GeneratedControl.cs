using System.Collections.Generic;
using System.Windows.Input;

namespace Acorisoft.FutureGL.MigaStudio.Controls
{
    public abstract class GeneratedControl : Control
    {
        protected static readonly SolidColorBrush Transparent;
        protected static readonly SolidColorBrush BG;
        protected static readonly SolidColorBrush Border;

        public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register(
            nameof(ItemWidth),
            typeof(double),
            typeof(GeneratedControl),
            new PropertyMetadata(24d));

        static GeneratedControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GeneratedControl), new FrameworkPropertyMetadata(typeof(GeneratedControl)));
            Transparent = new SolidColorBrush(Colors.Transparent);
            BG          = new SolidColorBrush(ThemeSystem.Instance.Theme.Colors[(int)ForestTheme.AdaptiveLevel2]);
            Border      = new SolidColorBrush(ThemeSystem.Instance.Theme.Colors[(int)ForestTheme.AdaptiveLevel3]);
        }

        private readonly List<FrameworkElement> _elements;
        private readonly object                 _sync;

        private bool         _initialized;
        private ItemsControl _items;

        protected GeneratedControl()
        {
            _sync     = new object();
            _elements = new List<FrameworkElement>(16);
        }

        /// <summary>
        /// 创建元素
        /// </summary>
        /// <param name="tag">指定当前的编号</param>
        /// <returns>返回一个新的Element</returns>
        protected abstract FrameworkElement GenerateElement();

        protected abstract void HighlightElement(FrameworkElement element);
        protected abstract void UnhighlightElement(FrameworkElement element);

        protected virtual void SetFirstElement(FrameworkElement element)
        {
        }

        protected void Rebuild()
        {
            lock (_sync)
            {
                if (_elements.Count > 0)
                {
                    _items.Items.Clear();

                    foreach (var element in _elements)
                    {
                        element.MouseDown -= OnElementMouseDown;
                    }

                    _elements.Clear();
                }

                var max   = Maximum;
                var width = 0d;

                for (var i = 0; i < max; i++)
                {
                    var element = GenerateElement();
                    element.Tag       =  i + 1;
                    element.MouseDown += OnElementMouseDown;

                    if (i == 0)
                    {
                        SetFirstElement(element);
                    }

                    width = element.Width;
                    _elements.Add(element);
                    _items.Items.Add(element);
                }

                HighlightElements(Value);
                MaxWidth = width * max;
            }
        }

        private void UnhighlightElements()
        {
            lock (_sync)
            {
                foreach (var element in _elements)
                {
                    UnhighlightElement(element);
                }
            }
        }

        private void HighlightElements(int index)
        {
            lock (_sync)
            {
                if (_elements.Count == 0)
                {
                    return;
                }

                for (var i = 0; i < index; i++)
                {
                    HighlightElement(_elements[i]);
                }
            }
        }

        private void OnElementMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is not FrameworkElement { Tag: int index })
            {
                return;
            }

            if (Value == index && 
                index == 1)
            {
                Value = 0;
            }
            else
            {
                Value = index;
            }
        }

        public override void OnApplyTemplate()
        {
            _items       = GetTemplateChild("Items") as ItemsControl;
            _initialized = true;
            Rebuild();


            base.OnApplyTemplate();
        }

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            nameof(Maximum),
            typeof(int),
            typeof(GeneratedControl),
            new PropertyMetadata(Boxing.IntValues[10], OnMaximumChanged));


        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(int),
            typeof(GeneratedControl),
            new PropertyMetadata(Boxing.IntValues[1], OnValueChanged));

        public double ItemWidth
        {
            get => (double)GetValue(ItemWidthProperty);
            set => SetValue(ItemWidthProperty, value);
        }
        
        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var gc    = (GeneratedControl)d;
            var value = (int)e.NewValue;

            if (!gc._initialized)
            {
                return;
            }

            gc.UnhighlightElements();
            gc.HighlightElements(value);
        }

        public int Value
        {
            get => (int)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var gc = (GeneratedControl)d;

            if (!gc._initialized)
            {
                return;
            }

            gc.Rebuild();
        }

        public int Maximum
        {
            get => (int)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }
    }

    public sealed class DegreeControl : GeneratedControl
    {
        protected override void SetFirstElement(FrameworkElement element)
        {
            if (element is Border b)
            {
                b.Margin = new Thickness(0);
            }
        }

        protected override FrameworkElement GenerateElement()
        {
            return new Border
            {
                Background          = Transparent,
                Width               = ItemWidth,
                Height              = 12,
                BorderBrush         = BG,
                BorderThickness     = new Thickness(1),
                UseLayoutRounding   = true,
                SnapsToDevicePixels = true,
                VerticalAlignment   = VerticalAlignment.Center,
                Margin              = new Thickness(-1, 0, 0, 0)
            };
        }

        protected override void HighlightElement(FrameworkElement element)
        {
            if (element is Border b)
            {
                b.Background      = Border;
                b.BorderBrush     = BG;
                b.BorderThickness = new Thickness(1, 0, 0, 0);
            }
        }

        protected override void UnhighlightElement(FrameworkElement element)
        {
            if (element is Border b)
            {
                b.BorderBrush     = BG;
                b.BorderThickness = new Thickness(1);
                b.Background      = Transparent;
            }
        }


        // F1 M24,24z M0,0z M20.84,4.61A5.5,5.5,0,0,0,13.06,4.61L12,5.67 10.94,4.61A5.5,5.5,0,0,0,3.16,12.39L4.22,13.45 12,21.23 19.78,13.45 20.84,12.39A5.5,5.5,0,0,0,20.84,4.61z
        // F1 M24,24z M0,0z M12,2L12,2 15.09,8.26 22,9.27 17,14.14 18.18,21.02 12,17.77 5.82,21.02 7,14.14 2,9.27 8.91,8.26 12,2z
    }
}