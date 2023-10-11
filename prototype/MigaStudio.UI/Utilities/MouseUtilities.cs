using System.Runtime.InteropServices;

namespace Acorisoft.FutureGL.MigaStudio.Utilities
{
    public static class MouseUtilities
    {
        public static Point CorrectGetPosition(Visual relativeTo)
        {
            var w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            return relativeTo.PointFromScreen(new Point(w32Mouse.X, w32Mouse.Y));
        }
        public static Point GetScreenPosition()
        {
            var w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            return new Point(w32Mouse.X, w32Mouse.Y);
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public readonly Int32 X;
            public readonly Int32 Y;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);
    }
}
