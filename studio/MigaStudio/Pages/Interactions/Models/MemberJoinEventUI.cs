using Acorisoft.FutureGL.MigaDB.Data.Socials;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions.Models
{
    public class MemberJoinEventUI : MessageUI, IMessageUpdateSupport
    {
        private readonly Func<string, string> _memberNameFinder;

        public MemberJoinEventUI(ChannelMessage msg, Func<string, string> memberNameFinder)
        {
            MessageSource     = msg ?? throw new ArgumentException(nameof(msg));
            MemberID          = msg.MemberID;
            _memberNameFinder = memberNameFinder ?? throw new ArgumentException(nameof(memberNameFinder));
            Update();
        }

        public void Update()
        {
            MemberName = _memberNameFinder(MessageSource.MemberID);
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
            get => MessageSource.Content;
            set
            {
                MessageSource.Content = value;
                RaiseUpdated();
            }
        }

        public override MessageBase Source => MessageSource;
    }

    public class MemberLeaveEventUI : MessageUI, IMessageUpdateSupport
    {
        private readonly Func<string, string> _memberNameFinder;

        public MemberLeaveEventUI(ChannelMessage msg, Func<string, string> memberNameFinder)
        {
            MessageSource     = msg ?? throw new ArgumentException(nameof(msg));
            _memberNameFinder = memberNameFinder ?? throw new ArgumentException(nameof(memberNameFinder));
            Update();
        }

        public void Update()
        {
            MemberName = _memberNameFinder(MessageSource.MemberID);
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
            get => MessageSource.Content;
            set
            {
                MessageSource.Content = value;
                RaiseUpdated();
            }
        }

        public override MessageBase Source => MessageSource;
    }
}