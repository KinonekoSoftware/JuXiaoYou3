namespace Acorisoft.FutureGL.MigaDB.Core
{
    public interface IDatabaseUpdater
    {
        bool Update(IDatabase database);
        int TargetVersion { get; }
        int ResultVersion { get; }
    }
}