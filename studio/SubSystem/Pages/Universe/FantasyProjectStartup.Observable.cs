using Acorisoft.FutureGL.MigaStudio.ViewModels.FantasyProject;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{
    public interface IProjectItemMessenger
    {
        void AddChild(ProjectItem item);
        void RemoveChild(ProjectItem item);
        
        ProjectItem Parent { get; }
        ProjectItem Current { get; }
    }
    
    partial class FantasyProjectStartupViewModel
    {
        private Dictionary<string, ProjectItem> AttachableObjectToProjectItemMapping = new Dictionary<string, ProjectItem>();
        private Dictionary<string, object>      ProjectItemToAttachableObjectMapping = new Dictionary<string, object>();
    }
}