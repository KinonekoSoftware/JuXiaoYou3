namespace Acorisoft.FutureGL.Forest.Controls
{
    public interface IForestControl : ITextResourceAdapter, IHighlightColorPalette
    {
        void InvalidateState();
    }
}