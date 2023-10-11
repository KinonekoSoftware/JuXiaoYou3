namespace Acorisoft.FutureGL.Forest.Controls
{
    public class Menu : ForestMenuBase
    {
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new MenuItem();
        }
    }

    public class ContextMenu : ForestContextMenuBase
    {
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new MenuItem();
        }
    }

    public class MenuItem : ForestMenuItemBase
    {
        


        protected override void StopAnimation()
        {
        }

        protected override void SetForeground(Brush brush)
        {
        }

        protected override void OnInvalidateState()
        {
        }

        protected override void GoToNormalState(HighlightColorPalette palette, ForestThemeSystem theme)
        {

        }

        protected override void GoToHighlight1State(Duration duration, HighlightColorPalette palette, ForestThemeSystem theme)
        {
            
        }

        protected override void GoToHighlight2State(Duration duration, HighlightColorPalette palette, ForestThemeSystem theme)
        {
            
        }

        protected override void GoToDisableState(HighlightColorPalette palette, ForestThemeSystem theme)
        {
            
        }

        protected override void GetTemplateChildOverride(ITemplatePartFinder finder)
        {
            
        }
    }
}