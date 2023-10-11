using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows.Data;
using Microsoft.Xaml.Behaviors;

namespace Acorisoft.FutureGL.Forest
{
    public class XamlAssist : DependencyObject
    {
        #region GroupName

        
        public static readonly DependencyProperty GroupNameProperty = DependencyProperty.RegisterAttached(
            "GroupNameOverride", 
            typeof(string), 
            typeof(XamlAssist), 
            new PropertyMetadata(default(string)));

        public static void SetGroupName(ItemsControl element, string value)
        {
            element.SetValue(GroupNameProperty, value);

            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            
            element.Loaded += OnApplyGroupStyle;
        }

        private static void OnApplyGroupStyle(object sender, RoutedEventArgs e)
        {
            var i   = (ItemsControl)sender;
            var cvs = CollectionViewSource.GetDefaultView(i.ItemsSource);

            if (cvs?.GroupDescriptions is null)
            {
                return;
            }
            
            cvs.GroupDescriptions.Clear();
            cvs.GroupDescriptions.Add(new PropertyGroupDescription(GetGroupName(i)));
        }

        public static string GetGroupName(ItemsControl element)
        {
            return (string)element.GetValue(GroupNameProperty);
        }

        #endregion

        #region IsDeferred

        public static readonly DependencyProperty IsDeferredProperty = DependencyProperty.RegisterAttached(
            "IsDeferredOverride", typeof(bool), typeof(XamlAssist), new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty TextProperty = DependencyProperty.RegisterAttached(
            "Text", typeof(string), typeof(XamlAssist), new PropertyMetadata(default(string)));

        public static void SetText(DependencyObject element, string value)
        {
            element.SetValue(TextProperty, value);
        }

        public static string GetText(DependencyObject element)
        {
            return (string)element.GetValue(TextProperty);
        }

        public static void SetIsDeferred(TextBox element, bool value)
        {
            var interaction = Interaction.GetBehaviors(element);
            if (interaction.Any(x => x is TextSampler))
            {
                value = false;
            }
            
            interaction.Add(new TextSampler());
            element.SetValue(IsDeferredProperty, Boxing.Box(value));
        }

        public static bool GetIsDeferred(TextBox element)
        {
            return (bool)element.GetValue(IsDeferredProperty);
        }

        #endregion
        
        protected class TextSampler : Behavior<TextBox>
        {
            private IDisposable _disposable;
            
            protected override void OnAttached()
            {
                _disposable = Observable.FromEventPattern<TextChangedEventHandler, TextChangedEventArgs>(
                                            added => AssociatedObject.TextChanged   += added,
                                            removed => AssociatedObject.TextChanged -= removed)
                                        .Throttle(TimeSpan.FromMilliseconds(500))
                                        .ObserveOn(Xaml.Get<IScheduler>())
                                        .Subscribe(_ =>
                                        {
                                            SetText(AssociatedObject, AssociatedObject.Text);
                                        });
            }

            protected override void OnDetaching()
            {
                _disposable?.Dispose();
                base.OnDetaching();
            }
        }
    }
}