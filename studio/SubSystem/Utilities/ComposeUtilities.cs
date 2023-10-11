using Acorisoft.FutureGL.Forest.Views;

namespace Acorisoft.FutureGL.MigaStudio.Utilities
{
    public static class ComposeUtilities
    {
        public static async Task AddComposeAsync(ComposeEngine engine)
        {
            var title = await SingleLineViewModel.String(SubSystemString.EditNameTitle);

            if (!title.IsFinished)
            {
                return;
            }

            var now = DateTime.Now;
            var item = new ComposeCache
            {
                Id             = ID.Get(),
                Name           = title.Value,
                TimeOfCreated  = now,
                TimeOfModified = now,
            };
            
            engine.AddComposeCache(item);
        }
        
        public static async Task AddComposeAsync(ICollection<ComposeCache> collection, ComposeEngine engine)
        {
            var title = await SingleLineViewModel.String(SubSystemString.EditNameTitle);

            if (!title.IsFinished)
            {
                return;
            }

            var now = DateTime.Now;
            var item = new ComposeCache
            {
                Id             = ID.Get(),
                Name           = title.Value,
                TimeOfCreated  = now,
                TimeOfModified = now,
            };
            
            collection.Add(item);
            engine.AddComposeCache(item);
        }
    }
}