namespace Acorisoft.FutureGL.MigaStudio.Editors
{
    public interface IWorkspaceCommandManager
    {
        void Heading(int level);
        void Bold();
        void Italic();
        void Table();
        void Image();
    }

    public interface IUndoRedoManager
    {
        bool CanUndo();
        bool CanRedo();

        void Undo();
        void Redo();
    }
}