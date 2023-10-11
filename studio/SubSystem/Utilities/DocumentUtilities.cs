using System.IO;
using System.Linq;
using Acorisoft.FutureGL.Forest.Views;
using Acorisoft.FutureGL.MigaDB.Data.Keywords;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.IO;
using Acorisoft.FutureGL.MigaStudio.Pages.Documents;
using Acorisoft.FutureGL.MigaStudio.Pages.Gallery;

namespace Acorisoft.FutureGL.MigaStudio.Utilities
{
    public static class DocumentUtilities
    {
        public static async Task AddDocument(DocumentEngine engine, Action<DocumentCache> callback)
        {
            var result = await AddDocument(engine);

            if (!result.IsFinished)
            {
                return;
            }

            var cache   = result.Value;

            if (!result.IsFinished)
            {
                await Xaml.Get<IBuiltinDialogService>()
                          .Notify(CriticalLevel.Warning,
                              SubSystemString.Notify,
                              result.Reason);
            }

            callback?.Invoke(cache);
        }

        public static async Task AddDocument(DocumentEngine engine, DocumentType type, Action<DocumentCache> callback)
        {
            var result = await AddDocument(engine, type);

            if (!result.IsFinished)
            {
                return;
            }

            var cache   = result.Value;

            if (!result.IsFinished)
            {
                await Xaml.Get<IBuiltinDialogService>()
                          .Notify(CriticalLevel.Warning,
                              SubSystemString.Notify,
                              result.Reason);
            }

            callback?.Invoke(cache);
        }
        
        public static async Task<Op<DocumentCache>> AddDocument(DocumentEngine engine)
        {
            var result = await NewDocumentViewModel.New();

            if (!result.IsFinished)
            {
                return Op<DocumentCache>.Failed("未选择");
            }

            var cache   = result.Value;
            var result1 = engine.AddDocumentCache(cache);

            return !result.IsFinished ? 
                Op<DocumentCache>.Failed(Language.GetEnum(result1.Reason)) :
                Op<DocumentCache>.Success(cache);
        }
        
        public static async Task<Op<DocumentCache>> AddDocument(DocumentEngine engine, DocumentType type)
        {
            var result = await NewDocumentViewModel.New(type);

            if (!result.IsFinished)
            {
                return Op<DocumentCache>.Failed("未选择");
            }

            var cache   = result.Value;
            var result1 = engine.AddDocumentCache(cache);

            return !result.IsFinished ? 
                Op<DocumentCache>.Failed(Language.GetEnum(result1.Reason)) :
                Op<DocumentCache>.Success(cache);
        }

        public static DocumentCache UpdateDocument(DocumentEngine engine, string id, IList<DocumentCache> sourceA, IList<DocumentCache> sourceB)
        {
            if (sourceA is null ||
                sourceB is null ||
                engine is null ||
                string.IsNullOrEmpty(id))
            {
                return null;
            }

            var indexA = sourceA.IndexOf(x => x.Id == id);
            var indexB = sourceB.IndexOf(x => x.Id == id);

            var inside = engine.GetCache(id);

            if (inside is null)
            {
                return null;
            }

            if(indexA > -1) sourceA[indexA] = inside;
            if (indexB > -1) sourceB[indexB] = inside;
            return inside;
        }

        public static void OpenDocument(string id)
        {
            var controller = Xaml.Get<TabBaseAppViewModel>()
                                 .CurrentController as TabController;
            var engine = Studio.Engine<DocumentEngine>();
            var cache  = engine.GetCache(id);
            OpenDocument(controller, cache);
        }

        public static void OpenDocument(TabController controller, DocumentCache cache)
        {
            if (cache is null || controller is null)
            {
                return;
            }

            switch (cache.Type)
            {
                case DocumentType.None:
                    break;
                case DocumentType.Skill:
                    controller.OpenDocument<SkillDocumentViewModel>(cache);
                    break;
                case DocumentType.Character:
                    controller.OpenDocument<CharacterDocumentViewModel>(cache);
                    break;
                case DocumentType.Item:
                    controller.OpenDocument<ItemDocumentViewModel>(cache);
                    break;
                case DocumentType.Geography:
                    controller.OpenDocument<GeographyDocumentViewModel>(cache);
                    break;
                case DocumentType.Other:
                    controller.OpenDocument<OtherDocumentViewModel>(cache);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// 修改头像
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="imageEngine"></param>
        /// <param name="cache"></param>
        /// <param name="callback"></param>
        public static async Task ChangeDocument(DocumentEngine engine, ImageEngine imageEngine, DocumentCache cache, Action<DocumentCache> callback)
        {
            if (cache is null ||
                engine is null ||
                imageEngine is null)
            {
                return;
            }

            var r = await ImageUtilities.Avatar();

            if (!r.IsFinished)
            {
                return;
            }

            var    buffer = r.Buffer;
            var    raw    = await Pool.MD5.ComputeHashAsync(buffer);
            var    md5    = Convert.ToBase64String(raw);
            string avatar;

            if (imageEngine.HasFile(md5))
            {
                var fr = imageEngine.Records.FindById(md5);
                avatar = fr.Uri;
            }
            else
            {
                avatar = ImageUtilities.GetAvatarName();
                buffer.Seek(0, SeekOrigin.Begin);
                imageEngine.WriteAvatar(buffer, avatar);

                var record = new FileRecord
                {
                    Id   = md5,
                    Uri  = avatar,
                    Type = ResourceType.Image,
                    Width = 256,
                    Height = 256
                };

                imageEngine.AddFile(record);
            }

            cache.Avatar = avatar;
            SynchronizeDocument(engine, cache);
        }

        /// <summary>
        /// 同步文档，将DocumentCache的变化应用到Document
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="cache"></param>
        public static void SynchronizeDocument(DocumentEngine engine, DocumentCache cache)
        {
            if (cache is null || engine is null)
            {
                return;
            }

            var document = engine.DocumentDB.FindById(cache.Id);

            if (document is null)
            {
                engine.UpdateDocument(cache);
            }
            else
            {
                SynchronizeDocument(cache, document);
                engine.UpdateDocument(document, cache);
            }
        }
        
        /// <summary>
        /// 同步文档，将DocumentCache的变化应用到Document
        /// </summary>
        /// <param name="document"></param>
        /// <param name="cache"></param>
        public static void SynchronizeDocument(DocumentCache cache, Document document)
        {
            if (cache is null || document is null)
            {
                return;
            }

            UpdateName(document, cache.Name);
            UpdateAvatar(document, cache.Avatar);
            UpdateIntro(document, cache.Intro);
            document.Owner = cache.Owner;
        }

        private static void UpdateName(Document document, string value)
        {
            document.Name = value;
            UpdateMetadata(document, MetadataUtilities.MetaNameOfName, value);
        }
        
        private static void UpdateAvatar(Document document, string value)
        {
            document.Avatar = value;
            UpdateMetadata(document, MetadataUtilities.MetaNameOfAvatar, value);
        }
        
        private static void UpdateIntro(Document document, string value)
        {
            document.Intro = value;
            UpdateMetadata(document, MetadataUtilities.MetaNameOfIntro, value);
        }

        private static void UpdateMetadata(Document document, string name, string value)
        {
            var index = document.Metas
                                .IndexOf(x => x.Name == name);

            if (index == -1)
            {
                document.Metas.Add(new Metadata
                {
                    Name = name,
                    Value = value,
                    Type = MetadataKind.Text
                });
            }
            else
            {
                var meta = document.Metas[index];

                if (meta.Type != MetadataKind.Text)
                {
                    document.Metas[index] = new Metadata
                    {
                        Name  = name,
                        Value = value,
                        Type  = MetadataKind.Text
                    };
                }
                else
                {
                    document.Metas[index]
                            .Value = value;
                }
            }

            if (document.Parts
                        .FirstOrDefault(x => x is PartOfBasic) is not PartOfBasic basic)
            {
                return;
            }

            basic.Buckets[name] = value;
        }

        public static void RemoveDocument(KeywordEngine ke, DocumentEngine engine, DocumentCache cache, Action<DocumentCache> callback)
        {
            if (cache is null || engine is null)
            {
                return;
            }

            ke.RemoveMappings(cache.Id);
            engine.RemoveDocumentCache(cache);
            callback?.Invoke(cache);
        }

        public static void LockOrUnlock(DocumentEngine engine, DocumentCache cache)
        {
            if (cache is null || engine is null)
            {
                return;
            }

            cache.IsLocked = !cache.IsLocked;
            engine.UpdateDocument(cache);
        }
    }
}