using System.Linq;
using Acorisoft.FutureGL.MigaDB;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Data.Keywords;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Data.Templates;
using NLog;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Composes
{
    public class ComposeEditorViewModel : ComposeEditorBase
    {
        protected override void IsDataPartExistence(Compose document)
        {
            base.IsDataPartExistence(document);
        }


        protected override void OnCreateDocument(Compose document)
        {
        }

        protected override void OnStart()
        {
#if DEBUG
           // this.Obsoleted(Language.GetText(Feature.TextEditorFeatureMissing), 10);
#else
            Studio.CheckFlag(Feature.TextEditorFeatureMissing,  async () =>
            {
                await this.Obsoleted(Language.GetText(Feature.TextEditorFeatureMissing), 10);
            });
#endif
            base.OnStart();
        }
    }
}