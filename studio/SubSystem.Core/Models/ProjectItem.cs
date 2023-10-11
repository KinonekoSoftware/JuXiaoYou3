using System.Collections;
using System.Windows;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels.FantasyProject
{
    public abstract class ProjectItem : StorageUIObject, ICollection<ProjectItem>
    {
        private readonly ObservableCollection<ProjectItem> _children;

        protected ProjectItem()
        {
            _children = new ObservableCollection<ProjectItem>();
            Children  = new ReadOnlyObservableCollection<ProjectItem>(_children);
        }
        
        #region ICollection<ProjectItem>
        
        
        public IEnumerator<ProjectItem> GetEnumerator()
        {
            return _children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_children).GetEnumerator();
        }

        public void Add(ProjectItem item)
        {
            if (item is null)
            {
                return;
            }
            
            item.Parent = this;
            _children.Add(item);
        }

        public void Clear()
        {
            _children.ForEach(x => x.Parent = null);
            _children.Clear();
        }

        public bool Contains(ProjectItem item)
        {
            return _children.Contains(item);
        }

        public void CopyTo(ProjectItem[] array, int arrayIndex)
        {
            _children.CopyTo(array, arrayIndex);
        }

        public bool Remove(ProjectItem item)
        { 
            if (item is null)
            {
                return false;
            }
            
            item.Parent = null;
            return _children.Remove(item);
        }

        public int Count => _children.Count;

        public bool IsReadOnly => ((ICollection<ProjectItem>)_children).IsReadOnly;
        
        #endregion
        
        public ProjectItem Parent { get; private set; }

        public abstract string ToolTips { get; set; }
        public abstract string Name { get; set; }

        /// <summary>
        /// 视图模型
        /// </summary>
        public Type ViewModelType { get; init; }
        
        /// <summary>
        /// 
        /// </summary>
        public TabViewModel ViewModel { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public FrameworkElement View { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public Func<ProjectItem, FrameworkElement> Expression { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public DocumentType Type { get; init; }
        
        /// <summary>
        /// 参数1
        /// </summary>
        public object Parameter1 { get; set; }
        
        /// <summary>
        /// 参数2
        /// </summary>
        public object Parameter2 { get; init; }
        
        /// <summary>
        /// 参数3
        /// </summary>
        public object Parameter3 { get; init; }
        
        /// <summary>
        /// 子级菜单
        /// </summary>
        public ObservableCollection<ProjectItem> Property { get; init; }
        
        /// <summary>
        /// 子级菜单
        /// </summary>
        public ReadOnlyObservableCollection<ProjectItem> Children { get; init; }
    }
}