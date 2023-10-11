using System.Drawing.Drawing2D;
using System.Linq;
using Acorisoft.FutureGL.MigaStudio.Editors;
using NLog;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Composes
{
    partial class ComposeEditorBase
    {
        private readonly Dictionary<Type, IWorkspace>     WorkspaceMapping;
        private readonly ObservableCollection<IWorkspace> InternalWorkspaceCollection;
        private          IWorkspace                       _workspace;

        private void ReleaseWorkspace()
        {
            WorkspaceCollection.ForEach(x =>
            {
                x.WorkspaceChanged = null;
                x.Dispose();
            });
        }

        #region CreateWorkspace

        protected void CreateWorkspace(PartOfMarkdown md)
        {
            if (md is null)
            {
                return;
            }

            var workspace = EditorUtilities.CreateFromMarkdownPart(md);
            workspace.WorkspaceChanged = OnWorkspaceChanged;

            if (!WorkspaceMapping.TryAdd(typeof(PartOfMarkdown), workspace))
            {
                Xaml.Get<ILogger>()
                    .Warn("创建了重复的RTF模组");
                return;
            }

            InternalWorkspaceCollection.Add(workspace);
        }

        #endregion

        private void OnWorkspaceChanged(StateChangedEventSource source, IWorkspace workspace)
        {
            if (source == StateChangedEventSource.Initialize)
            {
                
                //
                // 更新按钮状态
                UpdateUndoRedoCommandState();

                //
                // 统计
                Statistic(workspace);
            }
            else if (source == StateChangedEventSource.TextSource)
            {
                //
                // 更新按钮状态
                UpdateUndoRedoCommandState();

                //
                // 统计
                Statistic(workspace);

                //
                // 设置更改状态
                SetDirtyState();
            }
            else if (source == StateChangedEventSource.Caret)
            {
                //
                //
                workspace.UpdateCaretState();
                LineNumber = workspace.LineNumber;
                ColumnNumber = workspace.LineColumn;
            }
        }

        #region IComposeEditor

        ObservableCollection<IWorkspace> IComposeEditor.InternalWorkspaceCollection => InternalWorkspaceCollection;

        void IComposeEditor.Initialize()
        {
            //
            // 初始化
            WorkspaceCollection.ForEach(x =>
            {
                x.Scheduler = Scheduler;
                x.Initialize();
            });

            Workspace = WorkspaceCollection.FirstOrDefault();
            
            // 统计当前的字数
            Statistic(Workspace);
        }

        #endregion

        /// <summary>
        /// 获取或设置 <see cref="Workspace"/> 属性。
        /// </summary>
        public IWorkspace Workspace
        {
            get => _workspace;
            set
            {
                _workspace?.Inactive();
                SetValue(ref _workspace, value);
                _workspace?.Active();
                
                // re
                OnWorkspaceChanged(StateChangedEventSource.Initialize, value);
            }
        }

        public ReadOnlyObservableCollection<IWorkspace> WorkspaceCollection { get; }
    }
}