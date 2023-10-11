using System.IO;
using Acorisoft.FutureGL.MigaDB.Data.Socials;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions.Models
{
    public class ImageUI : UserBasedMessageUI
    {

        public ImageUI(ChannelMessage msg, MemberInformationFinder finder) : base(msg, finder)
        {
            if (msg is null ||
                msg.Type != MessageType.Image)
            {
                throw new ArgumentException(nameof(msg));
            }
        }
        public string ImageSource => MessageSource.Source;
        public bool IsExists => File.Exists(MessageSource.Source);
    }
    
    
    public class EmojiUI : UserBasedMessageUI
    {
        public EmojiUI(ChannelMessage msg, MemberInformationFinder finder) : base(msg, finder)
        {
            if (msg is null ||
                msg.Type != MessageType.Emoji)
            {
                throw new ArgumentException(nameof(msg));
            }
        }



        /// <summary>
        /// 获取或设置 <see cref="MemberName"/> 属性。
        /// </summary>
        public string MemberName { get; set; }


        public string ImageSource => MessageSource.Source;
        public bool IsExists => File.Exists(MessageSource.Source);
    }
}