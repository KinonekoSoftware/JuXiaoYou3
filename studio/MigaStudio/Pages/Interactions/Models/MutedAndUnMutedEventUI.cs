using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaUtils;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions.Models
{
    public class MutedAndUnMutedEventUI : MessageUI, IMessageUpdateSupport
    {
        private readonly Func<string, string>              _memberNameFinder;
        private readonly Func<MessageType, object, string> _contentFinder;
        private          string                            _content;

        public MutedAndUnMutedEventUI(ChannelMessage msg, Func<string, string> memberNameFinder, Func<MessageType, object, string> contentFinder)
        {
            MessageSource     = msg ?? throw new ArgumentException(nameof(msg));
            MemberID          = msg.MemberID;
            _memberNameFinder = memberNameFinder ?? throw new ArgumentException(nameof(memberNameFinder));
            _contentFinder    = contentFinder ?? throw new ArgumentException(nameof(contentFinder));
            Update();
        }

        public void Update()
        {
            MemberName = _memberNameFinder(MessageSource.MemberID);
            Content    = _contentFinder(MessageSource.Type, MessageSource.Timestamp);
            RaiseUpdated(nameof(MemberName));
        }

        public ChannelMessage MessageSource { get; }


        /// <summary>
        /// 获取或设置 <see cref="MemberName"/> 属性。
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// 获取或设置 <see cref="Content"/> 属性。
        /// </summary>
        public string Content
        {
            get => _content;
            set => SetValue(ref _content, value);
        }

        public override MessageBase Source => MessageSource;
    }
}