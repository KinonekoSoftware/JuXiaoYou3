namespace Acorisoft.FutureGL.Forest.Controls.Selectors
{
    public abstract partial class ForestMenuBase : Menu, ITextResourceAdapter
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
    
    public abstract partial class ForestContextMenuBase : ContextMenu, ITextResourceAdapter
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