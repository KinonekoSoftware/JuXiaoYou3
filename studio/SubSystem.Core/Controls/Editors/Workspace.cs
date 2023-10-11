using System.Reactive.Concurrency;
using System.Windows;
using Acorisoft.FutureGL.MigaStudio.Editors.Models;

namespace Acorisoft.FutureGL.MigaStudio.Editors
{
    public abstract class Workspace<TControl> : ObservableObject, IWorkspace 
        where TControl : FrameworkElement
    {
        private   IScheduler          _scheduler;
        private   TControl            _ctrl;
        
        protected DisposableCollector Disposable = new DisposableCollector();

        protected override void ReleaseManagedResources()
        {
            _scheduler = null;
            Disposable.Dispose();
        }
        
        
        public abstract bool CanUndo();
        public abstract bool CanRedo();
        public abstract void Undo();
        public abstract void Redo();

        public abstract void Initialize();
        public abstract void Inactive();
        public abstract void Active();
        public abstract void UpdateCaretState();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outlineModel"></param>
        public abstract void ScrollTo(IOutlineModel outlineModel);

        public abstract IEnumerable<IOutlineModel> GetOutlineModels();
        

        public int LineNumber { get; protected set; }
        public int LineColumn { get; protected set; }

        public IScheduler Scheduler
        {
            get => _scheduler;
            set
            {
                if (value is null)
                {
                    return;
                }

                _scheduler = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsWorking { get; protected set; }
        
        /// <summary>
        /// 
        /// </summary>
        public abstract string Content { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public WorkspaceChangedEventHandler WorkspaceChanged { get; set; }
        
        public TControl Control
        {
            get => _ctrl;
            set
            {
                if (value is null)
                {
                    return;
                }

                if (_ctrl is not null &&
                    ReferenceEquals(_ctrl, value))
                {
                    return;
                }

                _ctrl     = value;
                IsWorking = false;
                Disposable.Dispose();
                Disposable = new DisposableCollector();
            }
        }
    }
}