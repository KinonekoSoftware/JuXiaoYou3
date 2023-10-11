using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaStudio.Pages.Interactions.Models;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions.Dialogs
{
    public class NewTimestampViewModel : ImplicitDialogVM
    {
        private string _prefix;
        private string _suffix;

        protected override bool IsCompleted() => !string.IsNullOrEmpty(_prefix) &&
                                                 !string.IsNullOrEmpty(_suffix);

        protected override void OnStart(RoutingEventArgs parameter)
        {
            var a = parameter.Parameter.Args;
            if (a.Length > 0 && a[0] is TimestampUI ui)
            {
                Entity = ui;
                IsEdit = true;
            }
            
            base.OnStart(parameter);
        }

        protected override void Finish()
        {
            if (IsEdit)
            {
                Entity.Prefix = Prefix;
                Entity.Suffix = Suffix;
                Result        = Entity;
            }
            else
            {
                Result = new ChannelMessage
                {
                    Id              = ID.Get(),
                    Timestamp       = Suffix,
                    TimestampPrefix = Prefix,
                    Type = MessageType.Timestamp
                };
            }
        }

        protected override string Failed()
        {
            if (string.IsNullOrEmpty(_prefix))
            {
                return "时间前缀（例如：昨天）为空";
            }

            if (string.IsNullOrEmpty(_suffix))
            {
                return "时间（例如 19:31）为空";
            }

            return "";
        }

        public static Task<Op<ChannelMessage>> New()
        {
            return DialogService()
                       .Dialog<ChannelMessage, NewTimestampViewModel>(new Parameter(args: Array.Empty<object>()));
        }
        
        public static Task<Op<ChannelMessage>> Edit(TimestampUI message)
        {
            return DialogService()
                .Dialog<ChannelMessage, NewTimestampViewModel>(new Parameter
                {
                    Args = new object[]
                    {
                        message
                    }
                });
        }
        
        public TimestampUI Entity { get; private set; }
        public bool IsEdit { get; private set; }

        /// <summary>
        /// 获取或设置 <see cref="Suffix"/> 属性。
        /// </summary>
        public string Suffix
        {
            get => _suffix;
            set => SetValue(ref _suffix, value);
        }
        /// <summary>
        /// 获取或设置 <see cref="Prefix"/> 属性。
        /// </summary>
        public string Prefix
        {
            get => _prefix;
            set => SetValue(ref _prefix, value);
        }
    }
}