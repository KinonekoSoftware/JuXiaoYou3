namespace Acorisoft.FutureGL.Forest.Controls.Selectors
{
    public abstract class ForestListBoxBase : ListBox, ITextResourceAdapter
    {
        #region ITextResourceAdapter Members

        void ITextResourceAdapter.SetText(string text)
        {
        }

        void ITextResourceAdapter.SetToolTips(string text)
        {
            ToolTip = text;
        }

        #endregion
    }

    public abstract class ForestListViewBase : ListView, ITextResourceAdapter
    {
        #region ITextResourceAdapter Members

        void ITextResourceAdapter.SetText(string text)
        {
        }

        void ITextResourceAdapter.SetToolTips(string text)
        {
            ToolTip = text;
        }

        #endregion
    }
}