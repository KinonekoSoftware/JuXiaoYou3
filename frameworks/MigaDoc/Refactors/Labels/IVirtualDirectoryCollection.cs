namespace Acorisoft.Miga.Doc.Labels
{
    public interface IVirtualDirectoryCollection<in T> : INonredundant where T : class, IVirtualDirectory
    {
        bool AddDirectory(string directoryName, IObjectLabel label);
        
        void RemoveDirectory(T directory);
    }
}