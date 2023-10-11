using System.Linq;

namespace Acorisoft.FutureGL.MigaStudio.Core
{
    partial class ServiceViewContainer
    {
        private static readonly Dictionary<Type, bool>               _AllowDictionary     = new Dictionary<Type, bool>();

        public static void Prepare<TDataPart>(bool state = true) where TDataPart : PartOfDetail
        {
            _AllowDictionary.TryAdd(typeof(TDataPart), state);
        }
        
        public static bool IsDetailPartPrepared(object part)
        {
            return part is not null &&
                   _AllowDictionary.TryGetValue(part.GetType(), out var v) &&
                   v;
        }
    }
}