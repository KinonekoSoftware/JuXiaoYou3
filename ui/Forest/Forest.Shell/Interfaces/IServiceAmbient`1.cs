namespace Acorisoft.FutureGL.Forest.Interfaces
{
    public interface IServiceAmbient<in T>
    {
        void SetServiceProvider(T host);
    }
}