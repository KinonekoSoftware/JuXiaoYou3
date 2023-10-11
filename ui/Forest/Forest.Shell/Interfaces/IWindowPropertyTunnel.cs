namespace Acorisoft.FutureGL.Forest.Interfaces
{
    public interface IWindowPropertyTunnel
    {
        Action<WindowState> WindowState { get; set; }
    }
}