using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace Acorisoft.FutureGL.Forest.Controls.Selectors
{
    public class ShadowTabControl : ForestTabControlBase
    {
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ShadowTabItem();
        }
    }
    
    public class ShadowTabItem : ForestTabItemBase
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
        

        protected override void StopAnimation()
        {
            _storyboard?.Stop(_bd);
        }

        protected override void SetForeground(Brush brush)
        {
            _content.SetValue(TextElement.ForegroundProperty, brush);
        
            if (IsFilled)
            {
                _icon.Fill = brush;
            }
            else
            {
                _icon.Stroke          = brush;
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
            if (IsSelected)
            {
                _highlight2          ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.HighlightA3]);
                _foregroundHighlight ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundInHighlight]);
                
                //
                // 设置背景颜色
                _bd.Background = _highlight2;
        
                // 创建阴影
                _bd.Effect = BuildHighlightShadowEffect();
        
                // 设置文本颜色
                SetForeground(_foregroundHighlight);
            }
            else
            {
                
                _background ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundLevel1]);
                _foreground ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundLevel1]);
                //
                // 设置背景颜色
                _bd.Background = _background;
                _bd.Effect     = null;
        
                // 设置文本颜色
                SetForeground(_foreground);
            }
        }

        protected override void GoToHighlight1State(Duration duration, HighlightColorPalette palette, ForestThemeSystem theme)
        {
            _background          ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundLevel1]);
            _highlight           ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.HighlightA4]);
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
            _highlight           ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.HighlightA4]);
            _highlight2          ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.HighlightA3]);
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
                Children = new TimelineCollection { /*OpacityAnimation,*/ backgroundAnimation }
            };
        
            //
            // 开始动画
            _bd.BeginStoryboard(_storyboard, HandoffBehavior.SnapshotAndReplace, true);
        
            // 创建阴影
            _bd.Effect = BuildHighlightShadowEffect();
        
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
            _bd.Effect     = null;
        
            // 设置文本颜色
            SetForeground(_foregroundHighlight);
        }

        protected override void GetTemplateChildOverride(ITemplatePartFinder finder)
        {
            finder.Find<Border>(PART_BdName, x => _bd                   = x)
                .Find<Path>(PART_IconName, x => _icon                   = x)
                .Find<ContentPresenter>(PART_ContentName, x => _content = x);
        }
        
        
        private DropShadowEffect BuildHighlightShadowEffect()
        {
            // <DropShadowEffect x:Key = "Shadow.A3" Direction = "270" ShadowDepth = "4.5" BlurRadius = "14" Opacity = "0.42" Color = "{DynamicResource AccentA3}" />
            return new DropShadowEffect
            {
                Color       = _highlight2.Color,
                ShadowDepth = 4.5d,
                BlurRadius  = 14,
                Opacity     = 0.42d,
                Direction   = 270
            };
        }
    }

    public class ShadowListBox : ForestListBoxBase
    {
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ShadowListBoxItem();
        }
    }
    
    public class ShadowListBoxItem : ForestListBoxItemBase
    {
        private const string PART_BdName      = "PART_Bd";
        private const string PART_ContentName = "PART_Content";
        
        
        private Storyboard       _storyboard;
        private Border           _bd;
        private ContentPresenter _content;
        

        private SolidColorBrush _background;
        private SolidColorBrush _foreground;
        private SolidColorBrush _foregroundHighlight;
        private SolidColorBrush _highlight;
        private SolidColorBrush _highlight2;
        private SolidColorBrush _disabled;
        

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
        }

        protected override void GoToNormalState(HighlightColorPalette palette, ForestThemeSystem theme)
        {
            if (IsSelected)
            {
                _highlight2          ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.HighlightA3]);
                _foregroundHighlight ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundInHighlight]);
                
                //
                // 设置背景颜色
                _bd.Background = _highlight2;
        
                // 创建阴影
                _bd.Effect = BuildHighlightShadowEffect();
        
                // 设置文本颜色
                SetForeground(_foregroundHighlight);
            }
            else
            {
                
                _background ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundLevel1]);
                _foreground ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundLevel1]);
                //
                // 设置背景颜色
                _bd.Background = _background;
                _bd.Effect     = null;
        
                // 设置文本颜色
                SetForeground(_foreground);
            }
        }

        protected override void GoToHighlight1State(Duration duration, HighlightColorPalette palette, ForestThemeSystem theme)
        {
            _background          ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundLevel1]);
            _highlight           ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.HighlightA4]);
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
            _highlight           ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.HighlightA4]);
            _highlight2          ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.HighlightA3]);
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
                Children = new TimelineCollection { /*OpacityAnimation,*/ backgroundAnimation }
            };
        
            //
            // 开始动画
            _bd.BeginStoryboard(_storyboard, HandoffBehavior.SnapshotAndReplace, true);
        
            // 创建阴影
            _bd.Effect = BuildHighlightShadowEffect();
        
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
            _bd.Effect     = null;
        
            // 设置文本颜色
            SetForeground(_foregroundHighlight);
        }

        protected override void GetTemplateChildOverride(ITemplatePartFinder finder)
        {
            finder.Find<Border>(PART_BdName, x => _bd                   = x)
                .Find<ContentPresenter>(PART_ContentName, x => _content = x);
        }
        
        
        private DropShadowEffect BuildHighlightShadowEffect()
        {
            // <DropShadowEffect x:Key = "Shadow.A3" Direction = "270" ShadowDepth = "4.5" BlurRadius = "14" Opacity = "0.42" Color = "{DynamicResource AccentA3}" />
            return new DropShadowEffect
            {
                Color       = _highlight2.Color,
                ShadowDepth = 4.5d,
                BlurRadius  = 14,
                Opacity     = 0.2d,
                Direction   = 270
            };
        }
    }
}