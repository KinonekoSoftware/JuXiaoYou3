namespace Acorisoft.Miga.Doc.Engines
{
    public interface IDataRecoverySupport
    {
        Task RecoveryAsync(string payload);
    }
}