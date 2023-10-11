using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Acorisoft.FutureGL.MigaDB;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using NLog;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
    partial class DocumentEditorBase
    {
        #region OnLoad

        protected override void LoadDocumentAfter(DocumentCache cache, Document document)
        {
            //
            // 升级文档与缓存
            UpgradeDocument(cache, document);
            
            //
            // 用于检测BasicPart是否不存在，这个检测是运行时检查，不仅仅需要
            IsDataPartCoerceExistence();
            
            // 从文档中复制元数据，但不会再次添加元数据到文档。
            CopyMetadataFromDocument();
        }

        protected override bool OnDataPartAddingBefore(DataPart part)
        {
            if (part is PartOfModule pom)
            {
                ModuleParts.Add(pom);
                AddBlock(pom.Name, pom.Blocks);
                return true;
            }

            return false;
        }

        protected override void OnDataPartAddingAfter(DataPart part)
        {
            if (DataPartTrackerOfType.TryAdd(part.GetType(), part))
            {
                if (part is PartOfBasic pob)
                {
                    BasicPart = pob;
                }
                else if (part is PartOfDetail pod)
                {
                    DetailParts.Add(pod);
                }
                else if (part is PartOfPresentation pop)
                {
                    //
                    // 优先覆盖
                    PresentationPart           = pop;
                    IsOverridePresentationPart = true;
                }
                else if (part is PartOfManifest)
                {
                    InvisibleDataParts.Add(part);
                }
            }
        }

        private void CopyMetadataFromDocument()
        {
            foreach (var meta in Document.Metas)
            {
                AddMetadata(meta, false);
            }
            
            foreach (var metadata in BasicPart.Buckets)
            {
                AddMetadata(new Metadata
                {
                    Name  = metadata.Key,
                    Value = metadata.Value,
                    Type  = MetadataKind.Text,
                });
            }

            // 这一步是一个耗时的操作
            // Task.Run(() =>
            // {
            //     foreach (var module in ModuleParts)
            //     {
            //         foreach (var block in module.Blocks
            //                                     .Where(x => !string.IsNullOrEmpty(x.Metadata)))
            //         {
            //             AddMetadata(block.ExtractMetadata());
            //         }
            //     }
            // });
        }

        
        private void IsDataPartCoerceExistence()
        {
            //
            // 检查当前打开的文档是否缺失指定的DataPart

            if (BasicPart is null)
            {
                BasicPart = new PartOfBasic { Buckets = new Dictionary<string, string>() };

                //
                // Tracking
                Document.Parts.Add(BasicPart);
                DataPartTrackerOfType.TryAdd(BasicPart.GetType(), BasicPart);

                //
                // Initialize
                Name = Cache.Name;
            }

            GetPartOfPresentation();

            //
            //
            DocumentUtilities.SynchronizeDocument(Cache, Document);
        }

        private void GetPartOfPresentation()
        {
            if (PresentationPart is null)
            {
                //
                // 打开 PresentationPart
                var db = Studio.DatabaseManager()
                               .Database
                               .CurrentValue;

                PresentationPart           = GetPresentationPreset(db, Type);
                IsOverridePresentationPart = false;
            }
        }

        #endregion

        #region OnCreate

        protected override Document CreateDocument(Action<Document> callback)
        {
            var document = new Document
            {
                Id        = Cache.Id,
                Name      = Cache.Name,
                Version   = Feature.CurrentDocumentVersion,
                Removable = true,
                Type      = Type,
                Parts     = new DataPartCollection(),
                Metas     = new MetadataCollection(),
            };

            //
            //
            CreateDocumentFromManifest(document);
            Document = document;
            callback?.Invoke(document);
            DocumentEngine.AddDocument(document);
            return document;
        }

        private void CreateDocumentFromManifest(Document document)
        {
            var manifest = Studio.Database()
                                 .Get<PresetProperty>()
                                 .GetModulePreset(Type);

            if (Type != manifest?.Type)
            {
                return;
            }

            var iterators = manifest.Templates
                                    .Select(x => TemplateEngine.CreateModule(x));

            //
            //
            document.Parts.AddMany(iterators);
        }

        #endregion

        protected void UpgradeDocument(DocumentCache cache, Document document)
        {
            if (document.Version < Feature.CurrentDocumentVersion)
            {
                document.Metas
                        .Clear();
                document.Version = Feature.CurrentDocumentVersion;
                cache.Version    = document.Version;
                SetDirtyState();
            }
        }

        protected override Document GetDocumentById(string id)
        {
            return DocumentEngine.GetDocument(id);
        }
    }
}