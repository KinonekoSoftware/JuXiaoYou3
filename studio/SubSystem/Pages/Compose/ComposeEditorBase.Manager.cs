using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Data.Templates;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using NLog;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Composes
{
    [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
    partial class ComposeEditorBase
    {
        protected override Compose GetDocumentById(string id)
        {
            return ComposeEngine.GetCompose(id);
        }

        protected override void PrepareOpeningDocument(ComposeCache cache, Compose document)
        {
            ActivateAllEngines();
        }
        
        
        #region OnLoad

        protected override void LoadDocumentAfter(ComposeCache cache, Compose document)
        {
            AddMetadataWhenDocumentOpening();
        }


        private void AddMetadataWhenDocumentOpening()
        {
            // foreach (var metadata in BasicPart.Buckets)
            // {
            //     UpsertMetadataWithoutSave(metadata.Key, metadata.Value);
            // }
            //
            // foreach (var module in ModuleParts)
            // {
            //     foreach (var block in module.Blocks
            //                                 .Where(x => !string.IsNullOrEmpty(x.Metadata)))
            //     {
            //         AddMetadata(block.ExtractMetadata());
            //     }
            // }
        }
        
        protected override void IsDataPartExistence(Compose compose)
        {
            //
            // 检查当前打开的文档是否缺失指定的DataPart
            HasDataPart<PartOfMarkdown>(() => new PartOfMarkdown());
            HasDataPart<PartOfAlbum>(() => new PartOfAlbum());
        }

        #endregion

        #region OnCreate

        protected override Compose CreateDocument(Action<Compose> callback)
        {
            var document = new Compose
            {
                Id    = Cache.Id,
                Name  = Cache.Name,
                Parts = new DataPartCollection(),
                Metas = new MetadataCollection(),
            };

            //
            //
            Document = document;
            callback?.Invoke(document);
            CreateComposeImpl(document);
            ComposeEngine.AddCompose(document);
            return document;
        }


        private static void CreateComposeImpl(Compose document)
        {
            document.Parts.Add(new PartOfMarkdown());
            document.Parts.Add(new PartOfAlbum());
        }
        #endregion

    }
}