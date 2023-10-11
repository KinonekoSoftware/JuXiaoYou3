namespace Acorisoft.FutureGL.MigaDB.Exceptions
{
    public class UpdaterException : Exception
    {
        public UpdaterException()
        {
        }

        public UpdaterException(string message) : base(message)
        {
        }

        public UpdaterException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}