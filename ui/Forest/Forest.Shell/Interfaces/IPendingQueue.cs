namespace Acorisoft.FutureGL.Forest.Interfaces
{
    public interface IPendingQueue : ICollection<PendingVerbose>
    {
        
    }

    public abstract class PendingVerbose
    {
        public abstract void Run();
    }

    internal class PendingQueue : List<PendingVerbose>,  IPendingQueue
    {
        
    }
}