namespace Acorisoft.Miga.Doc.Engines
{
    public interface IDataBackupSupport
    {
        Task<string> BackupAsync();
    }
}