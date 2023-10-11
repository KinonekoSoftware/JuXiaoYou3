namespace Acorisoft.FutureGL.MigaStudio.Resources.Panels
{
    public class TabItemPanel : Panel
    {
        private int  _columns;

        /// <summary>
        /// Compute the desired size of this UniformGrid by measuring all of the
        /// children with a Document equal to a cell's portion of the given
        /// Document (e.g. for a 2 x 4 grid, the child Document would be
        /// Document.Width*0.5 x Document.Height*0.25).  The maximum child
        /// width and maximum child height are tracked, and then the desired size
        /// is computed by multiplying these maximums by the row and column count
        /// (e.g. for a 2 x 4 grid, the desired size for the UniformGrid would be
        /// maxChildDesiredWidth*2 x maxChildDesiredHeight*4).
        /// </summary>
        /// <param name="Document">Document</param>
        /// <returns>Desired size</returns>
        protected override Size MeasureOverride(Size Document)
        {
            _columns = InternalChildren.Count == 0 ? 1 : InternalChildren.Count;
            var childDocument = new Size(Math.Clamp(Document.Width / _columns, 60, 200) - 1.5, Document.Height);
            var maxChildDesiredWidth = 0.0;
            var maxChildDesiredHeight = 0.0;

            //  Measure each child, keeping track of maximum desired width and height.
            for (int i = 0, count = InternalChildren.Count; i < count; ++i)
            {
                var child = InternalChildren[i];

                // Measure the child.
                child.Measure(childDocument);

                var childDesiredSize = child.DesiredSize;

                if (maxChildDesiredWidth < childDesiredSize.Width)
                {
                    maxChildDesiredWidth = childDesiredSize.Width;
                }

                if (maxChildDesiredHeight < childDesiredSize.Height)
                {
                    maxChildDesiredHeight = childDesiredSize.Height;
                }
            }

            return new Size(maxChildDesiredWidth * _columns, maxChildDesiredHeight);
        }

        /// <summary>
        /// Arrange the children of this UniformGrid by distributing space evenly 
        /// among all of the children, making each child the size equal to a cell's
        /// portion of the given arrangeSize (e.g. for a 2 x 4 grid, the child size
        /// would be arrangeSize*0.5 x arrangeSize*0.25)
        /// </summary>
        /// <param name="arrangeSize">Arrange size</param>
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var w = Math.Clamp(arrangeSize.Width / _columns, 60, 200) - 1.5;
            var h = arrangeSize.Height;
            var xStep = 0d;


            // Arrange and Position each child to the same cell size
            foreach (UIElement child in InternalChildren)
            {
                child.Arrange(new Rect(xStep, 0, w, h));

                // only advance to the next grid cell if the child was not collapsed
                if (child.Visibility != Visibility.Collapsed)
                {
                    xStep += w;
                }
            }

            return arrangeSize;
        }
    }
}