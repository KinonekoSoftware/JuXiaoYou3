using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Acorisoft.FutureGL.Forest.Controls.Selectors;
using ListBox = Acorisoft.FutureGL.Forest.Controls.ListBox;
using ListBoxItem = Acorisoft.FutureGL.Forest.Controls.ListBoxItem;

namespace Acorisoft.FutureGL.MigaStudio.Core
{
    public class SubViewListBox : ListBox
    {
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new SubViewListBoxItem();
        }
    }

    public class SubViewListBoxItem : ListBoxItem
    {
        private Storyboard      _storyboard;
        private Border          PART_BG;
        private SolidColorBrush _background;
        private SolidColorBrush _foreground;
        private SolidColorBrush _foregroundInHighlight;
        private SolidColorBrush _highlight;
        private SolidColorBrush _highlight2;

        static SubViewListBoxItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SubViewListBoxItem), new FrameworkPropertyMetadata(typeof(SubViewListBoxItem)));
        }

        protected override void StopAnimation()
        {
            _storyboard?.Stop(PART_Bd);
            base.StopAnimation();
        }

        protected override void GetTemplateChildOverride(ITemplatePartFinder finder)
        {
            PART_BG = (Border)GetTemplateChild("PART_BG");
            base.GetTemplateChildOverride(finder);
        }

        protected override void OnInvalidateState()
        {
            _background            = null;
            _foreground            = null;
            _foregroundInHighlight = null;
            _highlight             = null;
            _highlight2            = null;
            base.OnInvalidateState();
        }

        protected override void GoToNormalState(HighlightColorPalette palette, ForestThemeSystem theme)
        {
            if (DataContext is HeaderedSubView hsv)
            {
                _highlight2 ??= hsv.HighlightColor;
            }
            else
            {
                _highlight2 ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.HighlightA5]);
            }

            _background ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundLevel3]);
            _foreground ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundLevel1]);

            //
            // 设置背景颜色
            PART_BG.Background = new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundLevel3]);
            PART_Bd.Background = IsSelected
                ? _highlight2
                : _background;

            // 设置文本颜色
            SetForeground(_foreground);
        }

        protected override void GoToHighlight1State(Duration duration, HighlightColorPalette palette, ForestThemeSystem theme)
        {
            //
            // Opacity 动画
            _highlight             ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.Overlay100]);
            _foregroundInHighlight ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundInHighlight]);

            var backgroundAnimation = new ColorAnimation
            {
                Duration = duration,
                From     = _background.Color,
                To       = _highlight.Color,
            };

            Storyboard.SetTarget(backgroundAnimation, PART_Bd);
            Storyboard.SetTargetProperty(backgroundAnimation, new PropertyPath("(Border.Background).(SolidColorBrush.Color)"));

            _storyboard = new Storyboard
            {
                Children = new TimelineCollection { backgroundAnimation }
            };

            //
            // 开始动画
            PART_Bd.BeginStoryboard(_storyboard, HandoffBehavior.SnapshotAndReplace, true);
        }

        protected override void GoToHighlight2State(Duration duration, HighlightColorPalette palette, ForestThemeSystem theme)
        {
            _highlight ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.Overlay100]);
            if (DataContext is HeaderedSubView hsv)
            {
                _highlight2 ??= hsv.HighlightColor;
            }
            else
            {
                _highlight2 ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.HighlightA5]);
            }

            _foregroundInHighlight ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundLevel1]);

            var backgroundAnimation = new ColorAnimation
            {
                Duration = duration,
                From     = _highlight.Color,
                To       = _highlight2.Color,
            };


            // Storyboard.SetTarget(OpacityAnimation, _bd);
            Storyboard.SetTarget(backgroundAnimation, PART_Bd);
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
            PART_BG.Background = new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundLevel2]);
            PART_Bd.BeginStoryboard(_storyboard, HandoffBehavior.SnapshotAndReplace, true);
        }
    }
}