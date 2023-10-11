namespace Acorisoft.FutureGL.MigaStudio.Editors
{
    public interface IComposeEditor
    {
        void Initialize();
        
        ObservableCollection<IWorkspace> InternalWorkspaceCollection { get; }
    }
}