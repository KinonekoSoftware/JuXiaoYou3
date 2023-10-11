using System.Xml.Linq;

namespace Acorisoft.Miga.Xml
{
    public class Compilation<T>
    {
        public bool IsFinished { get; init; }
        public XDocument Document { get; init; }
        public T Result { get; init; }
        public IReadOnlyList<ErrorArgs> Errors { get; init; }
        public long TCT { get; init; }
        public long TCM { get; init; }
    }
}