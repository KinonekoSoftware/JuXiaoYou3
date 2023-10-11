using Acorisoft.FutureGL.MigaDB.Data.Socials;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions.Models
{
    public class TimestampUI : MessageUI
    {
        public TimestampUI(ChannelMessage msg)
        {
            if (msg is null ||
                msg.Type != MessageType.Timestamp)
            {
                throw new ArgumentException(nameof(msg));
            }

            MemberID      = msg.MemberID;
            MessageSource = msg;
        }
        
        public ChannelMessage MessageSource { get; }


        /// <summary>
        /// 获取或设置 <see cref="Suffix"/> 属性。
        /// </summary>
        public string Suffix
        {
            get => MessageSource.Timestamp;
            set
            {
                MessageSource.Timestamp = value;
                RaiseUpdated();
            }
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Prefix"/> 属性。
        /// </summary>
        public string Prefix
        {
            get => MessageSource.TimestampPrefix;
            set
            {
                MessageSource.TimestampPrefix = value;
                RaiseUpdated();
            }
        }

        public override MessageBase Source => MessageSource;
    }
}