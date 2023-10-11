using System.Windows.Shapes;
using Acorisoft.FutureGL.Forest.Controls.Buttons;

namespace Acorisoft.FutureGL.Forest.UI.Tools
{
    public class ToolButton : ForestIconButtonBase
    {
        static ToolButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToolButton), new FrameworkPropertyMetadata(typeof(ToolButton)));
        }

        private const string           PART_BdName      = "PART_Bd";
        private const string           PART_ContentName = "PART_Content";
        private const string           PART_IconName    = "PART_Icon";
        private       Border           _bd;
        private       Path             _icon;
        private       ContentPresenter _content;
        private       Storyboard       _storyboard;

        private SolidColorBrush _backgroundBrush;
        private SolidColorBrush _foregroundBrush;
        private SolidColorBrush _backgroundHighlight1Brush;
        private SolidColorBrush _backgroundHighlight2Brush;
        private SolidColorBrush _foregroundDisabledBrush;


        protected override void OnInvalidateState()
        {
            _backgroundBrush           = null;
            _foregroundBrush           = null;
            _backgroundHighlight1Brush = null;
            _backgroundHighlight2Brush = null;
            _foregroundDisabledBrush   = null;
            InvalidateVisual();
        }

        protected sealed override void StopAnimation()
        {
            _storyboard?.Stop(_bd);
        }


        protected override void SetForeground(Brush foreground)
        {
            if (ShowText)
            {
                _content.ClearValue(TextElement.ForegroundProperty);
                _content.SetValue(TextElement.ForegroundProperty, foreground);
            }

            if (IsFilled)
            {
                _icon.StrokeThickness = 0;
                _icon.ClearValue(Path.FillProperty);
                _icon.Fill = foreground;
            }
            else
            {
                _icon.StrokeThickness = 1;
                _icon.ClearValue(Path.StrokeProperty);
                _icon.Stroke = foreground;
            }
        }

        protected override void GoToNormalState(HighlightColorPalette palette, ForestThemeSystem theme)
        {
            _backgroundBrush ??= new SolidColorBrush(Colors.Transparent);
            _foregroundBrush ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundLevel1]);

            _bd.Background = _backgroundBrush;
            SetForeground(_foregroundBrush);
        }

        protected override void GoToHighlight1State(Duration duration, HighlightColorPalette palette, ForestThemeSystem theme)
        {
            _backgroundBrush ??= new SolidColorBrush(Colors.Transparent);
            _foregroundBrush           ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundLevel1]);
            _backgroundHighlight1Brush ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.Overlay100]);

            // 白底变特殊色
            // 高亮色变白色
            var backgroundAnimation = new ColorAnimation
            {
                Duration = duration,
                From     = _backgroundBrush.Color,
                To       = _backgroundHighlight1Brush.Color,
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
            SetForeground(_foregroundBrush);
        }

        protected override void GoToHighlight2State(Duration duration, HighlightColorPalette palette, ForestThemeSystem theme)
        {
            _foregroundBrush           ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundLevel1]);
            _backgroundHighlight2Brush ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.Overlay200]);

            // 白底变特殊色
            // 高亮色变白色
            var backgroundAnimation = new ColorAnimation
            {
                Duration = duration,
                From     = _backgroundHighlight1Brush.Color,
                To       = _backgroundHighlight2Brush.Color,
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
            SetForeground(_foregroundBrush);
        }

        protected override void GoToDisableState(HighlightColorPalette palette, ForestThemeSystem theme)
        {
            _backgroundBrush = new SolidColorBrush(Colors.Transparent);
            _foregroundDisabledBrush = new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundDisabled]);

            _bd.Background = _backgroundBrush;
            SetForeground(_foregroundDisabledBrush);
        }

        protected override void GetTemplateChildOverride(ITemplatePartFinder finder)
        {
            if (ShowText)
            {
                finder.Find<Border>(PART_BdName, x => _bd                     = x)
                      .Find<Path>(PART_IconName, x => _icon                   = x)
                      .Find<ContentPresenter>(PART_ContentName, x => _content = x);
            }
            else
            {
                finder.Find<Border>(PART_BdName, x => _bd   = x)
                      .Find<Path>(PART_IconName, x => _icon = x);
            }
        }


        public static readonly DependencyProperty ShowTextProperty = DependencyProperty.Register(
            nameof(ShowText),
            typeof(bool),
            typeof(ToolButton),
            new PropertyMetadata(Boxing.False));

        public bool ShowText
        {
            get => (bool)GetValue(ShowTextProperty);
            set => SetValue(ShowTextProperty, Boxing.Box(value));
        }
    }
}