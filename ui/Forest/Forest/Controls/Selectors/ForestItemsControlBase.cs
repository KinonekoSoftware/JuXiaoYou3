namespace Acorisoft.FutureGL.Forest.Controls.Selectors
{
    public abstract class ForestItemsControlBase : ItemsControl, ITextResourceAdapter
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