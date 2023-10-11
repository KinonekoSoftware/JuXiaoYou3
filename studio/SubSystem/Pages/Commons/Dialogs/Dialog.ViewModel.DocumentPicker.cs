using System.Linq;
using Acorisoft.FutureGL.Forest.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public class DocumentPickerViewModel : ImplicitDialogVM
    {
        private DocumentCache _selected;

        public DocumentPickerViewModel()
        {
            Documents = new ObservableCollection<DocumentCache>();
        }

        protected override void OnStart(RoutingEventArgs parameter)
        {
            var p = parameter.Parameter;
            var a = p.Args;
            
            if (a[0] is IEnumerable<DocumentCache> enumerable)
            {
                Documents.AddMany(enumerable, true);
            }
        }

        protected override bool IsCompleted() => Selected is not null;

        protected override void Finish()
        {
            Result = Selected;
        }

        protected override string Failed() => SR.NotSelected;

        /// <summary>
        /// 获取或设置 <see cref="Selected"/> 属性。
        /// </summary>
        public DocumentCache Selected
        {
            get => _selected;
            set => SetValue(ref _selected, value);
        }
        
        public ObservableCollection<DocumentCache> Documents { get; }
    }

    public class MultiDocumentPickerViewModel : ImplicitDialogVM
    {
        private DocumentCache _selected;

        public MultiDocumentPickerViewModel()
        {
            Documents = new ObservableCollection<DocumentCache>();
        }

        protected override void OnStart(RoutingEventArgs parameter)
        {
            var p = parameter.Parameter;
            var a = p.Args;
            
            if (a[0] is IEnumerable<DocumentCache> enumerable)
            {
                Documents.AddMany(enumerable, true);
            }
        }

        protected override bool IsCompleted() => Selected is not null;

        protected override void Finish()
        {
            if (TargetElement is null)
            {
                return;
            }

            Result = TargetElement.SelectedItems
                                  .Cast<DocumentCache>()
                                  .ToArray();
        }

        protected override string Failed() => SubSystemString.Unknown;
        

        /// <summary>
        /// 获取或设置 <see cref="Selected"/> 属性。
        /// </summary>
        public DocumentCache Selected
        {
            get => _selected;
            set => SetValue(ref _selected, value);
        }
        
        public ObservableCollection<DocumentCache> Documents { get; }
        public ListBox TargetElement { get; set; }
    }
}