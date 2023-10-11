using System.Windows.Controls.Primitives;

namespace Acorisoft.FutureGL.Forest.Controls
{
    public class TreeView : ForestTreeViewBase
    {
        static TreeView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeView), new FrameworkPropertyMetadata(typeof(TreeView)));
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is Separator ||
                   item is TreeViewItem ||
                   base.IsItemItsOwnContainerOverride(item);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TreeViewItem();
        }
    }

    public class TreeViewItem : ForestTreeViewItemBase
    {
        private const string PART_BdName      = "PART_Bd";
        private const string PART_ContentName = "PART_Content";
        private const string ExpanderName     = "Expander";


        private Storyboard       _storyboard;
        private Border           _bd;
        private ContentPresenter _content;
        private ToggleButton     _button;

        private SolidColorBrush _background;
        private SolidColorBrush _foreground;
        private SolidColorBrush _foregroundInHighlight;
        private SolidColorBrush _foregroundDisabled;
        private SolidColorBrush _highlight;
        private SolidColorBrush _highlight2;
        private SolidColorBrush _disabled;

        static TreeViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeViewItem), new FrameworkPropertyMetadata(typeof(TreeViewItem)));
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TreeViewItem();
        }


        protected override void GetTemplateChildOverride(ITemplatePartFinder finder)
        {
            finder.Find<Border>(PART_BdName, x => _bd                     = x)
                  .Find<ToggleButton>(ExpanderName, x => _button          = x)
                  .Find<ContentPresenter>(PART_ContentName, x => _content = x);
        }

        protected override void StopAnimation()
        {
            _storyboard?.Stop(_bd);
        }

        protected override void SetForeground(Brush brush)
        {
            _button.Foreground = brush;
            _content.SetValue(TextElement.ForegroundProperty, brush);
        }

        protected override void OnInvalidateState()
        {
            _background            = null;
            _foreground            = null;
            _highlight             = null;
            _highlight2            = null;
            _disabled              = null;
            _foregroundInHighlight = null;
            _foregroundDisabled    = null;
            InvalidateVisual();
        }

        protected override void GoToNormalState(HighlightColorPalette palette, ForestThemeSystem theme)
        {
            _foreground ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundLevel2]);
            _background ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundLevel3]);
            _highlight2 ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.HighlightA4]);

            //
            // 设置背景颜色
            _bd.Background = IsSelected
                ? _highlight2
                : _background;

            // 设置文本颜色
            SetForeground(_foreground);
        }

        protected override void GoToHighlight1State(Duration duration, HighlightColorPalette palette, ForestThemeSystem theme)
        {
            //
            // Opacity 动画
            _highlight             ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.HighlightA5]);
            _foregroundInHighlight ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundInHighlight]);

            var backgroundAnimation = new ColorAnimation
            {
                Duration = duration,
                From     = _background.Color,
                To       = _highlight.Color,
            };

            Storyboard.SetTarget(backgroundAnimation, _bd);
            Storyboard.SetTargetProperty(backgroundAnimation, new PropertyPath("(Border.Background).(SolidColorBrush.Color)"));

            _storyboard = new Storyboard
            {
                Children = new TimelineCollection { backgroundAnimation }
            };

            //
            // 开始动画
            _bd.BeginStoryboard(_storyboard, HandoffBehavior.SnapshotAndReplace, true);

            // 设置文本颜色
            SetForeground(_foregroundInHighlight);
        }

        protected override void GoToHighlight2State(Duration duration, HighlightColorPalette palette, ForestThemeSystem theme)
        {
            _highlight             ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.HighlightA5]);
            _highlight2            ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.HighlightA4]);
            _foregroundInHighlight ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundInHighlight]);

            var backgroundAnimation = new ColorAnimation
            {
                Duration = duration,
                From     = _highlight.Color,
                To       = _highlight2.Color,
            };


            // Storyboard.SetTarget(OpacityAnimation, _bd);
            Storyboard.SetTarget(backgroundAnimation, _bd);
            // Storyboard.SetTargetProperty(OpacityAnimation, new PropertyPath(OpacityProperty));
            Storyboard.SetTargetProperty(backgroundAnimation, new PropertyPath("(Border.Background).(SolidColorBrush.Color)"));

            _storyboard = new Storyboard
            {
                Children = new TimelineCollection
                {
                    /*OpacityAnimation,*/ backgroundAnimation
                }
            };

            //
            // 开始动画
            _bd.BeginStoryboard(_storyboard, HandoffBehavior.SnapshotAndReplace, true);

            // 设置文本颜色
            SetForeground(_foregroundInHighlight);
        }

        protected override void GoToDisableState(HighlightColorPalette palette, ForestThemeSystem theme)
        {
            _disabled           =   new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundDisabled]);
            _foregroundDisabled ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundDisabled]);

            //
            // 设置背景颜色
            _bd.Background = _disabled;

            // 设置文本颜色
            SetForeground(_foregroundDisabled);
        }
    }
}