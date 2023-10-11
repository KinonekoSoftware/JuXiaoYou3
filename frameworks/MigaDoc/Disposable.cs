namespace Acorisoft.FutureGL.MigaUtils
{
    public class Disposable : IDisposable
    {
        protected virtual void ReleaseUnmanagedResources()
        {
        }

        protected virtual void ReleaseManagedResources()
        {
        }

        protected void Dispose(bool disposing)
        {
            ReleaseManagedResources();
            if (disposing)
            {
                ReleaseUnmanagedResources();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Disposable()
        {
            Dispose(false);
        }
    }
}