
namespace Acorisoft.Miga.Doc.Engines
{
    [Lazy]
    [GeneratedModules]
    public class ChannelService : StorageService
    {
        public ChannelService()
        {
            IsLazyMode = true;
        }


        protected internal override void OnRepositoryOpening(RepositoryContext context, RepositoryProperty property)
        {
            // DB      = context.Database.GetCollection<Channel>(Constants.cn_channel);
            // IndexDB = context.Database.GetCollection<ChannelIndex>(Constants.cn_index_channel);
        }

        protected internal override void OnRepositoryClosing(RepositoryContext context)
        {
        }
    }
}