using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Acorisoft.FutureGL.Forest.Attributes;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaStudio.Models;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public abstract class HierarchicalViewModel : MetadataEditable<DocumentCache, Document>
    {
        private HeaderedSubView      _selectedSubView;
        private FrameworkElement _subView;
        
        protected HierarchicalViewModel()
        {
            InternalSubViews = new ObservableCollection<HeaderedSubView>();
            SubViews         = new ReadOnlyCollection<HeaderedSubView>(InternalSubViews);
        }

        protected void InitializeSubView()
        {
            CreateSubViews(InternalSubViews);
            SelectedSubView = InternalSubViews.FirstOrDefault();
        }

        /// <summary>
        /// 创建子页面
        /// </summary>
        /// <param name="collection">集合</param>
        protected abstract void CreateSubViews(ICollection<HeaderedSubView> collection);
        
        /// <summary>
        /// 当子页面创建时
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected abstract void OnSubViewChanged(HeaderedSubView oldValue, HeaderedSubView newValue);
        
        /// <summary>
        /// 获取或设置 <see cref="SubView"/> 属性。
        /// </summary>
        public FrameworkElement SubView
        {
            get => _subView;
            protected set => SetValue(ref _subView, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="SelectedSubView"/> 属性。
        /// </summary>
        public HeaderedSubView SelectedSubView
        {
            get => _selectedSubView;
            set
            {
                OnSubViewChanged(_selectedSubView, value);
                SetValue(ref _selectedSubView, value);
            }
        }

        [NullCheck(UniTestLifetime.Constructor)]
        protected ObservableCollection<HeaderedSubView> InternalSubViews { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public ReadOnlyCollection<HeaderedSubView> SubViews { get; }
    }
}