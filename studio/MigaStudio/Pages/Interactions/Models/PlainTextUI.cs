using Acorisoft.FutureGL.MigaDB.Data.Socials;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions.Models
{
    public class PlainTextUI : UserBasedMessageUI
    {
        
        public PlainTextUI(ChannelMessage msg, MemberInformationFinder finder) : base(msg, finder)
        {
            if (msg is null ||
                msg.Type != MessageType.PlainText)
            {
                throw new ArgumentException(nameof(msg));
            }
        }
        
        public string Content
        {
            get => MessageSource.Content;
            set
            {
                MessageSource.Content = value;
                RaiseUpdated();
            }
        }
    }
}