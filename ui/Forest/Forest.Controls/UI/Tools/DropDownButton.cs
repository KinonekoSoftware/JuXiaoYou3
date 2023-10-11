using System.Windows.Controls.Primitives;

namespace Acorisoft.FutureGL.Forest.UI.Tools
{
    public class DropDownButton : ToolButton
    {
        protected override void OnClick()
        {
            if (ContextMenu != null)
            {
                ContextMenu.PlacementTarget = this;
                ContextMenu.Placement       = PlacementMode.Bottom;
                ContextMenu.IsOpen          = true;
            }
            base.OnClick();
        }
    }
}