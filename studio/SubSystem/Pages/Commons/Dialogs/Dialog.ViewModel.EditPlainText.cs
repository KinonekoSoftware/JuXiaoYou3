using Acorisoft.FutureGL.MigaUtils.Foundation;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons.Dialogs
{
    public class EditPlainTextViewModel : DialogViewModel
    {
        private string _content;
        private string _name;
        
        public static Task<Op<object>> Edit(StickyNote note)
        {
            if (note is null)
            {
                return Task.FromResult(Op<object>.Failed("参数为空"));
            }

            return DialogService().Dialog(new EditPlainTextViewModel(), new Parameter
            {
                Args = new object[] { note }
            });
        }

        protected override void OnStart(RoutingEventArgs parameter)
        {
            StickyNote = parameter.Parameter.Args[0] as StickyNote;
            if (StickyNote is not null)
            {
                Name    = StickyNote.Title;
                Content = StickyNote.Content;
            }
            base.OnStart(parameter);
        }

        public override void Cancel()
        {
            StickyNote.TimeOfModified = DateTime.Now;
            StickyNote.Content        = Content;
            StickyNote.Title          = Name;
            StickyNote.Intro          = string.IsNullOrEmpty(Content) ? string.Empty : Content.SubString(50);
            base.Cancel();
        }

        public StickyNote StickyNote { get; private set; }

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }
        /// <summary>
        /// 获取或设置 <see cref="Content"/> 属性。
        /// </summary>
        public string Content
        {
            get => _content;
            set => SetValue(ref _content, value);
        }
    }
}