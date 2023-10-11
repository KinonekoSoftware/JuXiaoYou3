using Acorisoft.FutureGL.MigaDB.Data;

namespace Acorisoft.FutureGL.MigaDB.Core.Maintainers
{
    public class ServicePropertyMaintainer : IDatabaseMaintainer
    {
        public void Maintain(IDatabase database)
        {
            database.IfSet<ColorServiceProperty>(new ColorServiceProperty
            {
                Mappings = new List<ColorMapping>(),
            });
        }
    }
}