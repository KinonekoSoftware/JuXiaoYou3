using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Resources.EmptyState
{
    public partial class DisableEmptyStateView : UserControl
    {
        public DisableEmptyStateView()
        {
            InitializeComponent();
        }


        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title),
            typeof(string),
            typeof(DisableEmptyStateView),
            new PropertyMetadata(Forest.Services.Language.GetText("emptyState.Feature.Title")));

        public static readonly DependencyProperty SubTitleProperty = DependencyProperty.Register(
            nameof(SubTitle),
            typeof(string),
            typeof(DisableEmptyStateView),
            new PropertyMetadata(Forest.Services.Language.GetText("emptyState.Feature.SubTitle")));

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