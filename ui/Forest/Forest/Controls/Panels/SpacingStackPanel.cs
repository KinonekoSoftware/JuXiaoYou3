namespace Acorisoft.FutureGL.Forest.Controls.Panels
{
    public class SpacingStackPanel : StackPanel
    {
        protected override Size MeasureOverride(Size Document)
        {
            var gap = Gap;
            if (gap > 0)
            {
                var spacing = Orientation == Orientation.Horizontal ? 
                    new Thickness(gap, 0, 0, 0) : 
                    new Thickness(0, gap, 0, 0);
                var index = 0;
                
                foreach (FrameworkElement children in Children)
                {
                    if (index++ == 0)
                    {
                        children.Margin = new Thickness(0);
                        continue;
                    }
                    
                    children.Margin = spacing;
                }
            }
            
            return base.MeasureOverride(Document);
        }

        public static readonly DependencyProperty GapProperty = DependencyProperty.Register(
            nameof(Gap),
            typeof(int),
            typeof(SpacingStackPanel),
            new FrameworkPropertyMetadata(Boxing.IntValues[0], FrameworkPropertyMetadataOptions.AffectsMeasure));

        public int Gap
        {
            get => (int)GetValue(GapProperty);
            set => SetValue(GapProperty, value);
        }
    }
}