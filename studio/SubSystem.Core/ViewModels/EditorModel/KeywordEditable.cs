using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest.Attributes;
using Acorisoft.FutureGL.MigaDB.Data.Keywords;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaStudio.Pages.Commons;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using Acorisoft.FutureGL.MigaUtils.Collections;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public abstract class KeywordEditable<TCache, TDocument> : DocumentEditable<TCache, TDocument>
        where TDocument : class, IData
        where TCache : class, IDataCache
    {
        protected KeywordEditable()
        {
            Keywords      = new ObservableCollection<Keyword>();
            KeywordEngine = Studio.Engine<KeywordEngine>();

            AddKeywordCommand    = AsyncCommand(AddKeywordImpl);
            RemoveKeywordCommand = AsyncCommand<Keyword>(RemoveKeywordImpl, x => x is not null);
        }

        protected void SynchronizeKeywords()
        {
            Keywords.AddMany(KeywordEngine.GetKeywords(Cache.Id), true);
        }

        private async Task AddKeywordImpl()
        {
            await KeywordUtilities.AddKeyword(
                Cache.Id,
                Keywords,
                KeywordEngine,
                SetDirtyState,
                this.WarningNotification);
        }

        private async Task RemoveKeywordImpl(Keyword item)
        {
            await KeywordUtilities.RemoveKeyword(
                item,
                Keywords,
                KeywordEngine,
                SetDirtyState,
                this.Error);
        }


        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddKeywordCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<Keyword> RemoveKeywordCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public KeywordEngine KeywordEngine { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<Keyword> Keywords { get; }
    }
}