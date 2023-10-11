
namespace Acorisoft.FutureGL.Forest.Controls.Selectors
{
    public class ChromeListBox : ForestListBoxBase
    {
        static ChromeListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChromeListBox), new FrameworkPropertyMetadata(typeof(ChromeListBox)));
        }
        
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ChromeListBoxItem();
        }
    }

    public class ChromeListBoxItem : ForestListBoxItemBase
    {
        private const string PART_BdName      = "PART_Bd";
        private const string PART_ContentName = "PART_Content";


        private Storyboard       _storyboard;
        private Border           _bd;
        private ContentPresenter _content;

        private SolidColorBrush _background;
        private SolidColorBrush _foreground;
        private SolidColorBrush _foregroundHighlight;
        private SolidColorBrush _foregroundDisabled;
        private SolidColorBrush _highlight;
        private SolidColorBrush _highlight2;
        private SolidColorBrush _disabled;

        protected override void GetTemplateChildOverride(ITemplatePartFinder finder)
        {
            finder.Find<Border>(PART_BdName, x => _bd                     = x)
                  .Find<ContentPresenter>(PART_ContentName, x => _content = x);
        }

        protected override void StopAnimation()
        {
            _storyboard?.Stop(_bd);
        }

        protected override void SetForeground(Brush brush)
        {
            _content.SetValue(TextElement.ForegroundProperty, brush);
        }

        protected override void OnInvalidateState()
        {
            _background          = null;
            _foreground          = null;
            _foregroundHighlight = null;
            _highlight           = null;
            _highlight2          = null;
            _disabled            = null;
            _foregroundDisabled  = null;
            InvalidateVisual();
        }

        protected override void GoToNormalState(HighlightColorPalette palette, ForestThemeSystem theme)
        {
            _foreground          ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundLevel1]);
            _background          ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundLevel5]);
            _foregroundHighlight ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundLevel1]);
            _highlight2          ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundLevel3]);

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
            //
            // Opacity 动画
            _highlight           ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundLevel2]);
            _foregroundHighlight ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundLevel1]);

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
            _highlight           ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundLevel2]);
            _highlight2          ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundLevel3]);
            _foregroundHighlight ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundLevel1]);

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
            _disabled           ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundDisabled]);
            _foregroundDisabled ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundDisabled]);
            
            //
            // 设置背景颜色
            _bd.Background = _disabled;

            // 设置文本颜色
            SetForeground(_foregroundDisabled);
        }
    }
}