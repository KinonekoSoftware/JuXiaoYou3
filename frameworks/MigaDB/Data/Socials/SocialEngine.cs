namespace Acorisoft.FutureGL.MigaDB.Data.Socials
{
    public class SocialEngine : DataEngine
    {
        protected override void OnDatabaseOpening(DatabaseSession session)
        {
            var db = session.Database;
            CharacterDB    = db.GetCollection<MemberCache>(Constants.Name_Chat_Character);
            ChannelCacheDB = db.GetCollection<ChannelCache>(Constants.Name_Chat_ChannelCache);
            UpvoteDB       = db.GetCollection<Upvote>(Constants.Name_Chat_Upvote);
            ThreadDB       = db.GetCollection<SocialThread>(Constants.Name_Chat_Thread);
            ChannelDB      = db.GetCollection<Channel>(Constants.Name_Chat_Channel);
        }

        protected override void OnDatabaseClosing()
        {
            ChannelCacheDB = null;
            CharacterDB    = null;
            UpvoteDB       = null;
            ThreadDB       = null;
            ChannelDB      = null;
        }

        public void AddCharacter(MemberCache character)
        {
            if (character is null)
            {
                return;
            }

            CharacterDB.Upsert(character);
        }
        
        public void RemoveCharacter(MemberCache character)
        {
            if (character is null)
            {
                return;
            }

            CharacterDB.Delete(character.Id);
        }

        public void AddChannel(Channel channel)
        {
            if (channel is null)
            {
                return;
            }

            ChannelDB.Upsert(channel);
        }
        
        public void AddChannel(ChannelCache channel)
        {
            if (channel is null)
            {
                return;
            }

            ChannelCacheDB.Upsert(channel);
        }
        
        public void RemoveChannel(ChannelCache channel)
        {
            if (channel is null)
            {
                return;
            }
            ChannelCacheDB.Delete(channel.Id);
        }
        
        public void RemoveChannel(Channel channel)
        {
            if (channel is null)
            {
                return;
            }
            ChannelDB.Delete(channel.Id);
        }
        
        public IEnumerable<ChannelCache> GetChannels(string character)
        {
            return ChannelCacheDB.Find(x => x.AvailableMembers
                                        .Any(y => y == character));
        }
        

        public Channel GetChannel(string id)
        {
            return ChannelDB.FindById(id);
        }

        public IEnumerable<MemberCache> GetMembers() => CharacterDB.FindAll();
        
        public ILiteCollection<Channel> ChannelDB { get; private set; }
        public ILiteCollection<ChannelCache> ChannelCacheDB { get; private set; }
        public ILiteCollection<SocialThread> ThreadDB { get; private set; }
        public ILiteCollection<MemberCache> CharacterDB { get; private set; }
        public ILiteCollection<Upvote> UpvoteDB { get; private set; }
    }
}