using Microsoft.Xaml.Behaviors;

namespace Acorisoft.FutureGL.Forest.Behaviors
{
    public class DoubleClickBehavior : Behavior<FrameworkElement>, ICommandSource
    {
        protected override void OnAttached()
        {
            AssociatedObject.MouseDown += OnMouseDoubleClick;
            base.OnAttached();
        }

        private void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                return;
            }
            
            if (e.ClickCount < 2)
            {
                return;
            }
            var cmd = Command;
            var cmdParam = CommandParameter;
            if (cmd?.CanExecute(cmdParam) == true)
            {
                cmd.Execute(cmdParam);
            }
        }

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            nameof(Command),
            typeof(ICommand),
            typeof(DoubleClickBehavior),
            new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register(
            nameof(CommandTarget),
            typeof(IInputElement),
            typeof(DoubleClickBehavior),
            new PropertyMetadata(default(IInputElement)));
        
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            nameof(CommandParameter),
            typeof(object),
            typeof(DoubleClickBehavior),
            new PropertyMetadata(default(object)));

        public object CommandParameter
        {
            get => (object)GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public IInputElement CommandTarget
        {
            get => (IInputElement)GetValue(CommandTargetProperty);
            set => SetValue(CommandTargetProperty, value);
        }
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }
    }
}