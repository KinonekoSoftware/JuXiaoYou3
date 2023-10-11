using System.Collections.Generic;
using System.Linq;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Controls;
using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions.Dialogs
{
    public class NewMemberViewModel : ImplicitDialogVM
    {
        private DocumentCache _selected;

        public NewMemberViewModel()
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
                                  .Select(x => new MemberCache
                                  {
                                      Id = x.Id,
                                      Avatar = x.Avatar,
                                      Name = x.Name
                                  })
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

        public static Task<Op<IEnumerable<MemberCache>>> Add(IReadOnlySet<string> pool)
        {
            var documents = Studio.Engine<DocumentEngine>()
                                  .GetCaches(DocumentType.Character)
                                  .Where(x => !pool.Contains(x.Id));
            return DialogService()
                .Dialog<IEnumerable<MemberCache>, NewMemberViewModel>(new Parameter
                {
                    Args = new object[]
                    {
                        documents
                    }
                });
        }
    }
}