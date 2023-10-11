using Acorisoft.FutureGL.MigaStudio.Utilities;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Composes
{
    public partial class ComposeEditorBase
    {
        
        private void OpenDocumentImpl(DocumentCache cache)
        {
            DocumentUtilities.OpenDocument(Controller, cache);
        }
        
        private void UndoImpl()
        {
            if (Workspace is null)
            {
                return;
            }
            
            Workspace.Undo();
            UpdateUndoRedoCommandState();
        }
        
        private void RedoImpl()
        {
            if (Workspace is null)
            {
                return;
            }
            
            Workspace.Redo();
            UpdateUndoRedoCommandState();
        }

        public bool CanUndo() => Workspace?.CanUndo() ?? false;
        public bool CanRedo() => Workspace?.CanRedo() ?? false;
        
        private void UpdateUndoRedoCommandState()
        {
            UpdateUndoRedoCommandStateHandler?.Invoke();
        }
        
        public Action UpdateUndoRedoCommandStateHandler { get; set; }
        
        
        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand SaveComposeCommand { get; }


        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand NewComposeCommand { get; }
        
        
        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand RedoCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand UndoCommand { get; }
        
        public RelayCommand<DocumentCache> OpenDocumentCommand { get; }
    }
}