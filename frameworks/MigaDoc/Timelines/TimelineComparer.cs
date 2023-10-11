namespace Acorisoft.Miga.Doc.Entities.Timelines
{
    public class TimelineComparer : Comparer<Timeline>
    {
        public override int Compare(Timeline x, Timeline y)
        {
            if (x is null)
            {
                return -1;
            }

            if (y is null)
            {
                return 1;
            }
            return x.Index - y.Index;
        }
    }
}