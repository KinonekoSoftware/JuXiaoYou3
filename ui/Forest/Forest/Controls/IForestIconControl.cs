namespace Acorisoft.FutureGL.Forest.Controls
{
    public interface IForestIconControl
    {
        bool HasIcon { get; }
        Geometry Icon { get; set; }
        double IconSize { get; set; }
        bool IsFilled { get; set; }
    }
}