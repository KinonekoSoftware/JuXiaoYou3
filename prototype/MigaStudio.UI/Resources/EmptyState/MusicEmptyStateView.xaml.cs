using System.Windows.Controls;
using System.Windows.Input;

namespace Acorisoft.FutureGL.MigaStudio.Resources.EmptyState
{
    public partial class MusicEmptyStateView : UserControl
    {
        public MusicEmptyStateView()
        {
            InitializeComponent();
        }
        
        
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title),
            typeof(string),
            typeof(MusicEmptyStateView),
            new PropertyMetadata(Forest.Services.Language.GetText("emptyState.Collection.Title")));

        public static readonly DependencyProperty SubTitleProperty = DependencyProperty.Register(
            nameof(SubTitle),
            typeof(string),
            typeof(MusicEmptyStateView),
            new PropertyMetadata(Forest.Services.Language.GetText("emptyState.Collection.SubTitle")));


        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            nameof(Command),
            typeof(ICommand),
            typeof(MusicEmptyStateView),
            new PropertyMetadata(default(ICommand)));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public string SubTitle
        {
            get => (string)GetValue(SubTitleProperty);
            set => SetValue(SubTitleProperty, value);
        }
        
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
    }
}