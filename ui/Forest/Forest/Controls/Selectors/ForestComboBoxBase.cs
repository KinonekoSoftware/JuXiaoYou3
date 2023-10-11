namespace Acorisoft.FutureGL.Forest.Controls.Selectors
{
    public abstract partial class ForestComboBoxBase : ComboBox, ITextResourceAdapter
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