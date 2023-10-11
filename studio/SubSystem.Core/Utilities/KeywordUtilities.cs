using System.IO;
using System.Linq;
using Acorisoft.FutureGL.Forest.Views;
using Acorisoft.FutureGL.MigaDB.Data.Keywords;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.IO;

namespace Acorisoft.FutureGL.MigaStudio.Utilities
{
    public static class KeywordUtilities
    {
        public static string KeywordTooMany => Language.GetText("text.KeywordTooMany");
        public static string AddKeywordTitle => Language.GetText("text.AddKeyword");
        public static string AreYouSureRemoveIt => Language.GetText("text.AreYouSureRemoveIt");
        
        public static async Task AddKeyword(
            string documentID,
            IList<Keyword> keywords,
            KeywordEngine KeywordEngine,
            Action<bool> SetDirtyState,
            Func<string, Task> Warning)
        {
            if (KeywordEngine.GetKeywordCount(documentID) >= 32)
            {
                await Warning(KeywordTooMany);
                return;
            }

            var r    = await SingleLineViewModel.String(AddKeywordTitle);

            if (!r.IsFinished)
            {
                return;
            }

            if (KeywordEngine.HasKeyword(documentID, r.Value))
            {
                await Warning(Language.ContentDuplicatedText);
                return;
            }

            var key = new Keyword
            {
                Id         = ID.Get(),
                DocumentId = documentID,
                Name       = r.Value
            };
            keywords.Add(key);
            KeywordEngine.AddKeyword(key);
            SetDirtyState(true);
        }
        
        
        public static async Task ForceAddKeyword(
            string documentID,
            Keyword key,
            KeywordEngine KeywordEngine,
            Action<bool> SetDirtyState,
            Func<string, Task> Warning)
        {
            if (KeywordEngine.GetKeywordCount(documentID) >= 32)
            {
                await Warning(KeywordTooMany);
                return;
            }

            if (KeywordEngine.HasKeyword(documentID, key.Name))
            {
                return;
            }

            KeywordEngine.AddKeyword(key);
            SetDirtyState(true);
        }

        public static async Task RemoveKeyword(
            Keyword item,
            IList<Keyword> keywords,
            KeywordEngine KeywordEngine,
            Action<bool> SetDirtyState,
            Func<string, Task<bool>> DangerousOperation)
        {
            if (!await DangerousOperation(AreYouSureRemoveIt))
            {
                return;
            }

            if (!keywords.Remove(item))
            {
                return;
            }

            keywords.Remove(item);
            KeywordEngine.RemoveKeyword(item);
            SetDirtyState(true);
        }
    }
}