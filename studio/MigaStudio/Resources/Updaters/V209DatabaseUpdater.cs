using System.IO;
using System.Linq;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data.DataParts;
using Acorisoft.FutureGL.MigaDB.Data.Keywords;
using Acorisoft.FutureGL.MigaDB.Data.Templates;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.IO;
using Acorisoft.FutureGL.MigaDB.Services;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaStudio.Models.Modules.ViewModels;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using Acorisoft.FutureGL.MigaUtils;
using Acorisoft.Miga.Doc.Documents;
using Acorisoft.Miga.Doc.Parts;
using LiteDB;
using DataPartCollection = Acorisoft.FutureGL.MigaDB.Data.DataParts.DataPartCollection;
using MetadataCollection = Acorisoft.FutureGL.MigaDB.Data.Metadatas.MetadataCollection;
using OldCompose = Acorisoft.Miga.Doc.Documents.Compose;
using NewCompose = Acorisoft.FutureGL.MigaDB.Documents.Compose;
using OldDocument = Acorisoft.Miga.Doc.Documents.Document;
using NewDocument = Acorisoft.FutureGL.MigaDB.Documents.Document;
using OldRepositoryProperty = Acorisoft.FutureGL.MigaStudio.Resources.Updaters.RepositoryProperty;
using NewRepositoryProperty = Acorisoft.FutureGL.MigaDB.Models.DatabaseProperty;

// ReSharper disable ConvertIfStatementToReturnStatement


namespace Acorisoft.FutureGL.MigaStudio.Resources.Updaters
{
    public static partial class V209DatabaseUpdater
    {
        public static async Task<EngineResult> Update(IDatabaseManager databaseManager, string databaseFilePath, string targetFilePath)
        {
            using (var session = Xaml.Get<IBusyService>()
                                     .CreateSession())
            {
                var exists = await IsFileExistence(session, databaseFilePath, targetFilePath);
                if (!exists.IsFinished)
                {
                    return exists;
                }

                var mainDatabaseFileName = Path.Combine(databaseFilePath, "main.mgdb");
                var indexFileName        = Path.Combine(databaseFilePath, "index.migaidx");

                //
                //
                await session.Await(Language.GetText(SR.Updating));
                var oldProperty = FromFile<OldRepositoryProperty>(indexFileName) ??
                                  new OldRepositoryProperty
                                  {
                                      Author      = UntitledField,
                                      EnglishName = UntitledField,
                                      Email       = UntitledField,
                                      Summary     = UntitledField,
                                      Name        = UntitledField,
                                      Backgrounds = new ObservableCollection<string>(),
                                  };

                var newProperty = Mapping(oldProperty);
                var src         = new LiteDatabase(mainDatabaseFileName);
                var r           = await databaseManager.CreateAsync(targetFilePath, newProperty);

                if (r.IsFinished)
                {
                    src.Mapper
                       .RegisterType(typeof(Miga.Doc.Parts.CustomDataPart), x => BsonMapper.Global.Serialize(x) ,Deserialize);
                    return await Updating(session, databaseManager, src, databaseFilePath);
                }

                return EngineResult.Failed(EngineFailedReason.InputDataError);
            }
        }

        private static object Deserialize(BsonValue value)
        {
            var document = value.AsDocument;
            document[litedb.ID] = typeof(CustomDataPart).FullName;
            return BsonMapper.Global.ToObject<CustomDataPart>(document);
        }

        private static async Task<EngineResult> IsFileExistence(IBusySession session, string databaseFilePath, string targetFilePath)
        {
            if (string.IsNullOrEmpty(databaseFilePath) ||
                string.IsNullOrEmpty(targetFilePath))
            {
                return EngineResult.Failed(EngineFailedReason.ParameterEmptyOrNull);
            }

            await session.Await(Language.GetText("text.Updating.FileExistence"));
            var mainDatabaseFileName = Path.Combine(databaseFilePath, "main.mgdb");
            var indexFileName        = Path.Combine(databaseFilePath, "index.migaidx");


            //
            // 完成检查
            if (!File.Exists(mainDatabaseFileName) ||
                !File.Exists(indexFileName))
            {
                return EngineResult.Failed(EngineFailedReason.InputSourceNotExists);
            }

            if (!Directory.Exists(targetFilePath))
            {
                return EngineResult.Failed(EngineFailedReason.ParameterDependencyEmptyOrNull);
            }

            return EngineResult.Successful;
        }

        private static async Task<EngineResult> Updating(IBusySession session, IDatabaseManager databaseManager, LiteDatabase oldDB, string databaseFilePath)
        {
            try
            {
                // 
                await MigratingModules(session, databaseManager, oldDB, databaseFilePath);

                // 迁移文档
                await MigratingDocuments(session, databaseManager, oldDB, databaseFilePath);

                // 迁移文章
                await MigratingComposes(session, databaseManager, oldDB);
            }
            catch (IOException)
            {
                return EngineResult.Failed(EngineFailedReason.InputSourceOccupied);
            }
            catch (Exception)
            {
                return EngineResult.Failed(EngineFailedReason.Unknown);
            }

            return EngineResult.Successful;
        }

        private static async Task MigratingDocuments(IBusySession session, IDatabaseManager databaseManager, LiteDatabase oldDB, string databaseFilePath)
        {
            await session.Await(Language.GetText("text.Updating.DocumentUpdating"));

            try
            {
                //
                // 
                var imageEngine          = databaseManager.GetEngine<ImageEngine>();
                var KeywordEngine        = databaseManager.GetEngine<KeywordEngine>();
                var documentEngine       = databaseManager.GetEngine<DocumentEngine>();
                var documentService      = oldDB.GetCollection<OldDocument>(Miga.Doc.Constants.cn_document);
                var documentIndexService = oldDB.GetCollection<DocumentIndex>(Miga.Doc.Constants.cn_index);
                var srcImageDir          = Path.Combine(databaseFilePath, "Images");

                foreach (var oldCache in documentIndexService.FindAll())
                {
                    var newCache = Transform(MigratingImage(oldCache.Avatar), oldCache);
                    var avatar   = ImageUtilities.GetAvatarName();

                    if (!string.IsNullOrEmpty(oldCache.Avatar))
                    {
                        //
                        // porting avatar to new 
                        var avatarFileName = Path.Combine(srcImageDir, newCache.Avatar);
                        var buffer         = await File.ReadAllBytesAsync(avatarFileName);
                        var raw            = Pool.MD5.ComputeHash(buffer);
                        var md5            = Convert.ToBase64String(raw);
                        var ms             = new MemoryStream(buffer);
                        imageEngine.AddFile(new FileRecord
                        {
                            Id     = md5,
                            Uri    = avatar,
                            Width  = 256,
                            Height = 256,
                            Type   = ResourceType.Image
                        });

                        newCache.Avatar = avatar;
                        imageEngine.WriteAvatar(ms, newCache.Avatar);
                        await ms.DisposeAsync();
                    }

                    documentEngine.AddDocumentCache(newCache);


                    if (oldCache.Keywords is not null)
                    {
                        foreach (var keyword in oldCache.Keywords)
                        {
                            if (KeywordEngine.GetKeywordCount(oldCache.Id) >= 32)
                            {
                                return;
                            }

                            if (KeywordEngine.HasKeyword(oldCache.Id, keyword))
                            {
                                return;
                            }

                            KeywordEngine.AddKeyword(new Keyword
                            {
                                Name       = keyword,
                                DocumentId = oldCache.Id,
                                Id         = ID.Get()
                            });
                        }
                    }
                }

                foreach (var oldDocument in documentService.FindAll())
                {
                    var newDocument = Transform(oldDocument);

                    if (newDocument is null)
                    {
                        continue;
                    }

                    //
                    //
                    documentEngine.AddDocument(newDocument);
                }
            }
            catch
            {
                await databaseManager.CloseAsync();
                oldDB.Dispose();
            }
        }

        public static string GetAvatarName() => string.Format(ImageUtilities.AvatarPattern, ID.Get());

        private static string MigratingImage(string oldPattern)
        {
            // miga://v2.img/QQ图片20230118233550.jpg
            var param = string.IsNullOrEmpty(oldPattern) ? string.Empty : oldPattern.Substring(14);
            return param;
        }

        public static string GetNewImageSource(string param, string srcDir, string dstDir)
        {
            var src = Path.Combine(srcDir, param);
            var res = GetAvatarName();
            var dst = Path.Combine(dstDir, res);
            File.Copy(src, dst, true);
            return res;
        }


        private static async Task MigratingModules(IBusySession session, IDatabaseManager databaseManager, LiteDatabase oldDB, string databaseFilePath)
        {
            await session.Await(Language.GetText("text.Updating.ModuleUpdating"));

            //
            // 
            var oldModuleEngine = oldDB.GetCollection<ModuleIndex>(Miga.Doc.Constants.cn_modules);
            var reader          = new DataPartReader();
            var newModuleEngine = databaseManager.GetEngine<TemplateEngine>();


            foreach (var index in oldModuleEngine.FindAll())
            {
                var fileName = Path.Combine(databaseFilePath, "Modules", index.FileName);
                var result   = await reader.ReadFromAsync(fileName);
                if (!result.IsFinished)
                {
                    continue;
                }

                var oldModule = result.Result;
                var newModule = ModuleBlockFactory.Upgrade(oldModule);

                newModuleEngine.AddModule(newModule);
            }
        }

        private static async Task MigratingComposes(IBusySession session, IDatabaseManager databaseManager, LiteDatabase oldDB)
        {
            await session.Await(Language.GetText("text.Updating.ComposeUpdating"));

            var documentEngine       = databaseManager.GetEngine<ComposeEngine>();
            var documentService      = oldDB.GetCollection<OldCompose>(Miga.Doc.Constants.cn_compose);
            var documentIndexService = oldDB.GetCollection<ComposeIndex>(Miga.Doc.Constants.cn_index_compose);

            foreach (var oldCompose in documentIndexService.FindAll())
            {
                var newCompose = new ComposeCache
                {
                    Id             = oldCompose.Id,
                    Name           = oldCompose.Name,
                    IsDeleted      = false,
                    IsLocked       = false,
                    Version        = 1,
                    TimeOfCreated  = oldCompose.CreatedDateTime,
                    TimeOfModified = oldCompose.ModifiedDateTime,
                    Intro          = oldCompose.Summary,
                };
                documentEngine.AddComposeCache(newCompose);
            }
            
            foreach (var oldCompose in documentService.FindAll())
            {
                var newCompose = new NewCompose
                {
                    Id    = oldCompose.Id,
                    Name  = oldCompose.Name,
                    Parts = new DataPartCollection(),
                    Metas = new MetadataCollection(),
                };

                newCompose.Parts.Add(new PartOfMarkdown
                {
                    Content = oldCompose.Current
                                        ?.Content,
                });
                documentEngine.AddCompose(newCompose);
            }
        }

        public const string NameField     = "Name";
        public const string UntitledField = "Untitiled";
    }
}