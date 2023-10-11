using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.MigaDB.Services;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using Acorisoft.FutureGL.MigaUtils;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public partial class SettingViewModel
    {
        private static FolderCounter CreateApplicationCounter()
        {
            return new FolderCounter
            {
                Directory = AppDomain.CurrentDomain.BaseDirectory,
                Name      = Language.GetText("global.appSelf")
            };
        }
        
        private static FolderCounter CreateLogCounter()
        {
            return new FolderCounter
            {
                Directory = Xaml.Get<ApplicationModel>()
                                .Logs,
                Name = Language.GetText("global.logs")
            };
        }
        
        
        private static FolderCounter CreateFeedbackCounter()
        {
            return new FolderCounter
            {
                Directory = Xaml.Get<ApplicationModel>()
                                .Feedbacks,
                Name = Language.GetText("global.feedback")
            };
        }
        
        private static DatabaseCounter CreateSelfCounter(FolderCounter app, FolderCounter logs, FolderCounter feedback)
        {
            return new DatabaseCounter
            {
                Name = Language.GetText("global.appSelf"),
                Counters = new ObservableCollection<FolderCounter>
                {
                    app,
                    logs,
                    feedback
                },
                Directory = app.Directory
            };
        }
        
        private static DatabaseCounter CreateDatabaseCounter()
        {
            return new DatabaseCounter
            {
                Counters = new ObservableCollection<FolderCounter>(),
                Directory = Studio.DatabaseManager()
                                  .Database
                                  .CurrentValue
                                  .DatabaseDirectory,
                Name = Language.GetText("__Universe"),
            };
        }
        
        
        private void OnDataSourceChanged(ValueTuple<long, EngineCounter[]> x)
        {
            var (dbRootSize, engines) = x;
            Self.Size                 = Self.Counters
                                            .Sum(a => a.Size);
            
            Self.Counters
                .ForEach(e => e.Percent = e.Size / (double)Self.Size * 100);

            //
            //
            DatabaseCounter.Counters
                           .AddMany(engines, true);
            DatabaseCounter.Size = engines.Sum(e => e.Size) + dbRootSize;
            DatabaseCounter.Counters
                           .ForEach(c => c.Percent = c.Size / (double)DatabaseCounter.Size * 100);
        }

        private void OpenCounter(FolderCounter x)
        {
            Process.Start(new ProcessStartInfo
            {
                UseShellExecute = true,
                FileName        = "explorer.exe",
                Arguments       = x.Directory
            });
        }
        
        public void ComputeDirectorySize()
        {
            Task.Run(async () =>
            {
                foreach (var counter in Self.Counters)
                {
                    counter.Size = await IOSystemUtilities.GetFolderSize(counter.Directory, true);
                }
                
                var engines = Studio.DatabaseManager()
                                    .GetEngines()
                                    .OfType<FileEngine>()
                                    .Select(x => new EngineCounter
                                    {
                                        Directory = x.FullDirectory,
                                        Name      = Language.GetText(x.GetTextSource())
                                    })
                                    .ToArray();
                var databaseDirSize = await IOSystemUtilities.GetFolderSize(DatabaseCounter.Directory, false);
                foreach (var engine in engines)
                {
                    engine.Size = await IOSystemUtilities.GetFolderSize(engine.Directory, false);
                }

                _subject.OnNext(new ValueTuple<long, EngineCounter[]>(databaseDirSize, engines));
            });
            
        }

        private async Task CompressLogsImpl()
        {
            var savedlg = Studio.Save(Studio.ZipFilter, Studio.ZipExt);
            if (savedlg.ShowDialog() != true)
            {
                return;
            }

            try
            {
                await ZipFile.Zip(Logs.Directory, savedlg.FileName);
                await this.Successful("保存成功");
            }
            catch(Exception ex)
            {
                await this.Danger(ex.Message);
            }
        }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public DatabaseCounter DatabaseCounter { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public DatabaseCounter Self { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public FolderCounter Application { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public FolderCounter Logs { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public FolderCounter FeedBacks { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand RefreshCommand { get; }


        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<FolderCounter> OpenCommand { get; }
        public AsyncRelayCommand CompressLogsCommand { get; }
    }
}