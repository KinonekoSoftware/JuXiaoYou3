using System.Windows.Shapes;

namespace Acorisoft.FutureGL.Forest.UI.Tools
{
    public class TabControl : ForestTabControlBase
    {
        static TabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TabControl), new FrameworkPropertyMetadata(typeof(TabControl)));
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TabItem();
        }
    }

    public class TabItem : ForestTabItemBase
    {
        private const string PART_BdName      = "PART_Bd";
        private const string PART_ContentName = "PART_Content";
        private const string PART_IconName    = "PART_Icon";


        private Storyboard       _storyboard;
        private Border           _bd;
        private ContentPresenter _content;
        private Path             _icon;

        private SolidColorBrush _background;
        private SolidColorBrush _foreground;
        private SolidColorBrush _foregroundHighlight;
        private SolidColorBrush _highlight;
        private SolidColorBrush _highlight2;
        private SolidColorBrush _disabled;

        protected override void GetTemplateChildOverride(ITemplatePartFinder finder)
        {
            finder.Find<Border>(PART_BdName, x => _bd                     = x)
                  .Find<Path>(PART_IconName, x => _icon                   = x)
                  .Find<ContentPresenter>(PART_ContentName, x => _content = x);
        }


        protected override void StopAnimation()
        {
            _storyboard?.Stop(_bd);
        }

        protected override void SetForeground(Brush foreground)
        {
            _content.SetValue(TextElement.ForegroundProperty, foreground);

            if (IsFilled)
            {
                _icon.Fill = foreground;
            }
            else
            {
                _icon.Stroke          = foreground;
                _icon.StrokeThickness = 1d;
            }
        }

        protected override void OnInvalidateState()
        {
            _background          = null;
            _foreground          = null;
            _foregroundHighlight = null;
            _highlight           = null;
            _highlight2          = null;
            _disabled            = null;
        }

        protected override void GoToNormalState(HighlightColorPalette palette, ForestThemeSystem theme)
        {
            _highlight2          ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.HighlightA4]);
            _background          ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundLevel3]);
            _foreground          ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundLevel1]);
            _foregroundHighlight ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundInHighlight]);
            
            //
            // 设置背景颜色
            _bd.Background = IsSelected
                ? _highlight2
                : _background;

            // 设置文本颜色
            SetForeground(IsSelected
                ? _foregroundHighlight
                : _foreground);
        }

        protected override void GoToHighlight1State(Duration duration, HighlightColorPalette palette, ForestThemeSystem theme)
        {
            _highlight           ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.HighlightA5]);
            _foregroundHighlight ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundInHighlight]);
            
            //
            // Opacity 动画

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
            SetForeground(_foregroundHighlight);
        }

        protected override void GoToHighlight2State(Duration duration, HighlightColorPalette palette, ForestThemeSystem theme)
        {
            _highlight ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.HighlightA4]);
            _highlight2          ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.HighlightA5]);
            _foregroundHighlight ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundInHighlight]);
            
            //
            // Opacity 动画
            // var OpacityAnimation = new DoubleAnimation()
            // {
            //     Duration = duration,
            //     From     = 0.5,
            //     To       = 1,
            // };

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
            SetForeground(_foregroundHighlight);
        }

        protected override void GoToDisableState(HighlightColorPalette palette, ForestThemeSystem theme)
        {
            _disabled            ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundDisabled]);
            _foregroundHighlight ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundInHighlight]);

            //
            // 设置背景颜色
            _bd.Background = _disabled;

            // 设置文本颜色
            SetForeground(_foregroundHighlight);
        }
    }
}