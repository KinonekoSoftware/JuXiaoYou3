namespace Acorisoft.FutureGL.Forest.Models
{
    public enum DragDropState
    {
        DragStart,
        Dropped,
    }
    
    public class WindowDragDropArgs : EventArgs
    {
        public DragDropState State { get; init; }
        public DragEventArgs Args { get; init; }
    }
}