using System.Windows.Input;

namespace Acorisoft.FutureGL.MigaStudio.Controls
{
    public class DocumentCard : Control
    {
        static DocumentCard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DocumentCard), new FrameworkPropertyMetadata(typeof(DocumentCard)));
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title),
            typeof(string),
            typeof(DocumentCard),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty IdProperty = DependencyProperty.Register(
            nameof(Id),
            typeof(string),
            typeof(DocumentCard),
            new PropertyMetadata(default(string)));


        public static readonly DependencyProperty TimeOfCreatedProperty = DependencyProperty.Register(
            nameof(TimeOfCreated),
            typeof(DateTime),
            typeof(DocumentCard),
            new PropertyMetadata(default(DateTime)));


        public static readonly DependencyProperty TimeOfModifiedProperty = DependencyProperty.Register(
            nameof(TimeOfModified),
            typeof(DateTime),
            typeof(DocumentCard),
            new PropertyMetadata(default(DateTime)));


        public static readonly DependencyProperty ContentLengthProperty = DependencyProperty.Register(
            nameof(ContentLength),
            typeof(int),
            typeof(DocumentCard),
            new PropertyMetadata(Boxing.IntValues[0]));

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            nameof(Command),
            typeof(ICommand),
            typeof(DocumentCard),
            new PropertyMetadata(default(ICommand)));


        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            nameof(CommandParameter),
            typeof(object),
            typeof(DocumentCard),
            new PropertyMetadata(default(object)));

        public object CommandParameter
        {
            get => (object)GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }
        public int ContentLength
        {
            get => (int)GetValue(ContentLengthProperty);
            set => SetValue(ContentLengthProperty, value);
        }

        public DateTime TimeOfModified
        {
            get => (DateTime)GetValue(TimeOfModifiedProperty);
            set => SetValue(TimeOfModifiedProperty, value);
        }

        public DateTime TimeOfCreated
        {
            get => (DateTime)GetValue(TimeOfCreatedProperty);
            set => SetValue(TimeOfCreatedProperty, value);
        }
        
        public string Id
        {
            get => (string)GetValue(IdProperty);
            set => SetValue(IdProperty, value);
        }
        
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
    }
}