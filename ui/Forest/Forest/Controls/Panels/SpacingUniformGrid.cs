using System.Windows.Controls.Primitives;

namespace Acorisoft.FutureGL.Forest.Controls.Panels
{
    public class SpacingUniformGrid : UniformGrid
    {
        protected override Size MeasureOverride(Size Document)
        {
            var gap     = Gap;
            var rows    = Rows;
            var columns = Columns;
            var skip    = gap <= 0 && rows == 0 && columns == 0;
            var index   = 0;
            
            if (!skip)
            {
                if (rows > 0 && columns == 0)
                {
                    // 垂直布局
                    foreach (FrameworkElement child in Children)
                    {
                        child.Margin = new Thickness(0, 0, 0, gap);
                    }
                }
                else
                {
                    foreach (FrameworkElement child in Children)
                    {
                        if (index < columns)
                        {
                            child.Margin = index == 0 ? new Thickness(0) : new Thickness(gap, 0, 0, 0);
                        }
                        else
                        {
                            child.Margin = new Thickness(gap, gap, 0, 0);
                        }

                        index++;
                    }
                }
            }
            
            return base.MeasureOverride(Document);
        }


        public static readonly DependencyProperty GapProperty = DependencyProperty.Register(
            nameof(Gap),
            typeof(int),
            typeof(SpacingUniformGrid),
            new FrameworkPropertyMetadata(Boxing.IntValues[0], FrameworkPropertyMetadataOptions.AffectsMeasure));

        public int Gap
        {
            get => (int)GetValue(GapProperty);
            set => SetValue(GapProperty, value);
        }
    }
}