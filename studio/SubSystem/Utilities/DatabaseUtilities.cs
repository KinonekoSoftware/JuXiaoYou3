using Acorisoft.FutureGL.MigaDB.Core;

namespace Acorisoft.FutureGL.MigaStudio.Utilities
{
    public static class DatabaseUtilities
    {
        public static IDatabase Database => Studio.DatabaseManager()
                                                .Database
                                                .CurrentValue;
    }
}