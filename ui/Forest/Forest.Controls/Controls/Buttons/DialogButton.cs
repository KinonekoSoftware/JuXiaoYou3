using System.Windows.Shapes;

namespace Acorisoft.FutureGL.Forest.Controls.Buttons
{
    /// <summary>
    /// 这是一个特殊用途的按钮
    /// </summary>
    public class DialogButton : ForestIconButtonBase
    {
        public static readonly DependencyProperty PurposeProperty = DependencyProperty.Register(
            nameof(Purpose),
            typeof(ButtonPurpose),
            typeof(DialogButton),
            new PropertyMetadata(ButtonPurpose.CallToAction));
        

        public static Color GetPurposeColor(ButtonPurpose purpose, int level)
        {
            var colors = ThemeSystem.Instance.Theme.Colors;

            return level switch
            {
                2 => purpose switch
                {
                    ButtonPurpose.CallToAction  => colors[(int)ForestTheme.HighlightA4],
                    ButtonPurpose.CallToAction2 => colors[(int)ForestTheme.HighlightB4],
                    ButtonPurpose.CallToAction3 => colors[(int)ForestTheme.HighlightC4],
                    ButtonPurpose.CallToClose   => colors[(int)ForestTheme.Danger200],
                    ButtonPurpose.Close         => colors[(int)ForestTheme.SlateGray200],
                    ButtonPurpose.Warning       => colors[(int)ForestTheme.Warning200],
                    ButtonPurpose.Info          => colors[(int)ForestTheme.Info200],
                    ButtonPurpose.Success       => colors[(int)ForestTheme.Success200],
                    ButtonPurpose.Obsolete      => colors[(int)ForestTheme.Obsolete200],
                    _                           => colors[(int)ForestTheme.HighlightA4],
                },
                3 => purpose switch
                {
                    ButtonPurpose.CallToAction  => colors[(int)ForestTheme.HighlightA5],
                    ButtonPurpose.CallToAction2 => colors[(int)ForestTheme.HighlightB5],
                    ButtonPurpose.CallToAction3 => colors[(int)ForestTheme.HighlightC5],
                    ButtonPurpose.CallToClose   => colors[(int)ForestTheme.Danger300],
                    ButtonPurpose.Close         => colors[(int)ForestTheme.SlateGray300],
                    ButtonPurpose.Warning       => colors[(int)ForestTheme.Warning300],
                    ButtonPurpose.Info          => colors[(int)ForestTheme.Info300],
                    ButtonPurpose.Success       => colors[(int)ForestTheme.Success300],
                    ButtonPurpose.Obsolete      => colors[(int)ForestTheme.Obsolete300],
                    _                           => colors[(int)ForestTheme.HighlightA5],
                },
                4 => purpose switch
                {
                    ButtonPurpose.CallToAction  => colors[(int)ForestTheme.HighlightA1],
                    ButtonPurpose.CallToAction2 => colors[(int)ForestTheme.HighlightB1],
                    ButtonPurpose.CallToAction3 => colors[(int)ForestTheme.HighlightC1],
                    ButtonPurpose.CallToClose   => colors[(int)ForestTheme.Danger400],
                    ButtonPurpose.Close         => colors[(int)ForestTheme.SlateGray400],
                    ButtonPurpose.Warning       => colors[(int)ForestTheme.Warning400],
                    ButtonPurpose.Info          => colors[(int)ForestTheme.Info400],
                    ButtonPurpose.Success       => colors[(int)ForestTheme.Success400],
                    ButtonPurpose.Obsolete      => colors[(int)ForestTheme.Obsolete400],
                    _                           => colors[(int)ForestTheme.HighlightA1],
                },
                _ => purpose switch
                {
                    ButtonPurpose.CallToAction  => colors[(int)ForestTheme.HighlightA3],
                    ButtonPurpose.CallToAction2 => colors[(int)ForestTheme.HighlightB3],
                    ButtonPurpose.CallToAction3 => colors[(int)ForestTheme.HighlightC3],
                    ButtonPurpose.CallToClose   => colors[(int)ForestTheme.Danger100],
                    ButtonPurpose.Close         => colors[(int)ForestTheme.SlateGray100],
                    ButtonPurpose.Warning       => colors[(int)ForestTheme.Warning100],
                    ButtonPurpose.Info          => colors[(int)ForestTheme.Info100],
                    ButtonPurpose.Success       => colors[(int)ForestTheme.Success100],
                    ButtonPurpose.Obsolete      => colors[(int)ForestTheme.Obsolete100],
                    _                           => colors[(int)ForestTheme.HighlightA3],
                }
            };
        }


        private const string PART_BdName      = "PART_Bd";
        private const string PART_ContentName = "PART_Content";
        private const string PART_IconName    = "PART_Icon";

        private Border           _bd;
        private ContentPresenter _content;
        private Path             _icon;
        private Storyboard       _storyboard;

        private SolidColorBrush _backgroundBrush;
        private SolidColorBrush _foregroundBrush;
        private SolidColorBrush _backgroundHighlight1Brush;
        private SolidColorBrush _backgroundHighlight2Brush;
        private SolidColorBrush _foregroundHighlightBrush;
        private SolidColorBrush _backgroundDisabledBrush;
        private SolidColorBrush _foregroundDisabledBrush;


        protected override void OnInvalidateState()
        {
            _backgroundBrush           = null;
            _foregroundBrush           = null;
            _backgroundHighlight1Brush = null;
            _foregroundHighlightBrush  = null;
            _backgroundHighlight2Brush = null;
            _backgroundDisabledBrush   = null;
            _foregroundDisabledBrush   = null;
            InvalidateVisual();
        }

        protected sealed override void StopAnimation()
        {
            _storyboard?.Stop(_bd);
        }


        protected override void SetForeground(Brush foreground)
        {
            _content.SetValue(TextElement.ForegroundProperty, foreground);

            if (IsFilled)
            {
                _icon.StrokeThickness = 0;
                _icon.Fill            = foreground;
            }
            else
            {
                _icon.StrokeThickness = 2;
                _icon.Stroke          = foreground;
            }
        }

        protected override void GoToNormalState(HighlightColorPalette palette, ForestThemeSystem theme)
        {
            var purpose = Purpose;

            if (purpose == ButtonPurpose.Close)
            {
                _backgroundBrush ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundLevel3]);
                _foregroundBrush ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.SlateGray100]);
            }
            else
            {
                _backgroundBrush ??= new SolidColorBrush(GetPurposeColor(purpose, 1));
                _foregroundBrush ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundInHighlight]);
            }

            _bd.Background = _backgroundBrush;
            SetForeground(_foregroundBrush);
        }

        protected override void GoToHighlight1State(Duration duration, HighlightColorPalette palette, ForestThemeSystem theme)
        {
            //
            // Opacity 动画
            var purpose = Purpose;

            _backgroundBrush ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundLevel3]);
            _foregroundBrush ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.SlateGray100]);
            _foregroundHighlightBrush  ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundInHighlight]);
            _backgroundHighlight1Brush ??= new SolidColorBrush(GetPurposeColor(purpose, 2));

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
            SetForeground(_foregroundHighlightBrush);
        }

        protected override void GoToHighlight2State(Duration duration, HighlightColorPalette palette, ForestThemeSystem theme)
        {
            //
            // Opacity 动画
            var purpose = Purpose;

            _foregroundHighlightBrush  ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundInHighlight]);
            _backgroundHighlight2Brush ??= new SolidColorBrush(GetPurposeColor(purpose, 3));

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
            SetForeground(_foregroundHighlightBrush);
        }

        protected override void GoToDisableState(HighlightColorPalette palette, ForestThemeSystem theme)
        {
            var purpose = Purpose;

            _backgroundDisabledBrush ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundLevel3]);
            _foregroundDisabledBrush ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.SlateGray400]);

            _bd.Background = _backgroundDisabledBrush;
            SetForeground(_foregroundDisabledBrush);
        }


        protected override void GetTemplateChildOverride(ITemplatePartFinder finder)
        {
            finder.Find<Border>(PART_BdName, x => _bd                     = x)
                  .Find<ContentPresenter>(PART_ContentName, x => _content = x)
                  .Find<Path>(PART_IconName, x => _icon                   = x);
        }


        public ButtonPurpose Purpose
        {
            get => (ButtonPurpose)GetValue(PurposeProperty);
            set => SetValue(PurposeProperty, value);
        }
    }
}