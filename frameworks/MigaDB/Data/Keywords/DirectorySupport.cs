using System.Collections;
using System.Collections.ObjectModel;
using System.Security.AccessControl;

namespace Acorisoft.FutureGL.MigaDB.Data.Keywords
{
    public abstract class DirectorySupport : StorageObject
    {
        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 深度
        /// </summary>
        public int Height { get; init; }
    }

    public class DirectoryNode : DirectorySupport
    {
        /// <summary>
        /// 获取或设置 <see cref="Owner"/> 属性。
        /// </summary>
        public string Owner { get; set; }
    }

    public class DirectoryRoot : DirectorySupport
    {
    }


    public abstract class DirectorySupportUI : ObservableObject, ICollection<DirectorySupportUI>
    {
        #region Children / ICollection<DirectoryUI>

        

        public IEnumerator<DirectorySupportUI> GetEnumerator()
        {
            return Children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Children).GetEnumerator();
        }

        public void Add(DirectorySupportUI item)
        {
            Children.Add(item);
        }

        public void Clear()
        {
            Children.Clear();
        }

        public bool Contains(DirectorySupportUI item)
        {
            return Children.Contains(item);
        }

        public void CopyTo(DirectorySupportUI[] array, int arrayIndex)
        {
            Children.CopyTo(array, arrayIndex);
        }

        public bool Remove(DirectorySupportUI item)
        {
            return Children.Remove(item);
        }

        public int Count => Children.Count;

        public bool IsReadOnly => ((ICollection<DirectorySupportUI>)Children).IsReadOnly;
        
        /// <summary>
        /// 子级
        /// </summary>
        public ObservableCollection<DirectorySupportUI> Children { get; init; }
        
        #endregion
        
        
        public abstract int Height { get; }
        public abstract string Id { get; }
        public abstract string Name { get; set; }
    }

    public class DirectoryNodeUI : DirectorySupportUI
    {
        
        /// <summary>
        /// 
        /// </summary>
        public DirectoryNode Source { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public DirectorySupportUI Parent { get; set; }

        public override int Height => Source.Height;

        /// <summary>
        /// 获取或设置名字
        /// </summary>
        public sealed override string Name
        {
            get => Source.Name;
            set
            {
                Source.Name = value;
                RaiseUpdated();
            }
        }

        public sealed override string Id => Source.Id;

        /// <summary>
        /// 
        /// </summary>
        public string Owner
        {
            get => Source.Owner;
            set => Source.Owner = value;
        }
    }

    public class DirectoryRootUI : DirectorySupportUI
    {
        public sealed override string Id => Source.Id;
        
        /// <summary>
        /// 
        /// </summary>
        public DirectoryRoot Source { get; init; }
        
        public override int Height => Source.Height;

        /// <summary>
        /// 获取或设置名字
        /// </summary>
        public sealed override  string Name
        {
            get => Source.Name;
            set
            {
                Source.Name = value;
                RaiseUpdated();
            }
        }
    }
}