using System.Windows.Controls.Primitives;

namespace Acorisoft.FutureGL.Forest
{
    public class TextResourceFactory : ITextResourceFactory
    {
        public class WpfAdapter : ITextResourceAdapter
        {
            public void SetText(string text)
            {
                if (Element is ButtonBase button)
                {
                    button.Content = text;
                }
                else if (Element is HeaderedContentControl header)
                {
                    header.Header = text;
                }
                else if (Element is HeaderedItemsControl header2)
                {
                    header2.Header = text;
                }
                else if (Element is Label label)
                {
                    label.Content = text;
                }
                else if (Element is TextBlock tb)
                {
                    tb.Text = text;
                }
                else if (Element is Window window)
                {
                    window.Title = text;
                }
                else if (Element is GroupItem gi)
                {
                    gi.Content = text;
                }
            }

            public void SetToolTips(string text)
            {
                Element.ToolTip = text;
            }

            public FrameworkElement Element { get; init; }
        }

        public TextResourceFactory()
        {
            Factory = new Dictionary<Type, Func<FrameworkElement, ITextResourceAdapter>>
            {
                { typeof(HeaderedContentControl), x => new WpfAdapter { Element = x } },
                { typeof(GroupBox), x => new WpfAdapter { Element = x } },
                { typeof(MenuItem), x => new WpfAdapter { Element = x } },
                { typeof(Label), x => new WpfAdapter { Element = x } },
                { typeof(TextBlock), x => new WpfAdapter { Element = x } },
                { typeof(Button), x => new WpfAdapter { Element = x } },
                { typeof(ToggleButton), x => new WpfAdapter { Element = x } },
                { typeof(RadioButton), x => new WpfAdapter { Element = x } },
                { typeof(CheckBox), x => new WpfAdapter { Element = x } },
                { typeof(Expander), x => new WpfAdapter { Element = x } },
                { typeof(TextBox), x => new WpfAdapter { Element = x } },
                { typeof(TabItem), x => new WpfAdapter { Element = x } },
                { typeof(GroupItem), x => new WpfAdapter { Element = x } },
                { typeof(Window), x => new WpfAdapter { Element = x } },
            };
        }

        public ITextResourceAdapter Adapt(FrameworkElement instance)
        {
            return Factory.TryGetValue(instance.GetType(), out var factory) ? factory(instance) : new WpfAdapter { Element = instance };
        }

        public Dictionary<Type, Func<FrameworkElement, ITextResourceAdapter>> Factory { get; }
    }
}