using System.Reactive.Concurrency;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.Forest.Services;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaStudio.Core;
using DryIoc;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace MigaStudio.Tests
{
    [TestClass]
    public class ApplicationUnitTest
    {
        [ClassInitialize]
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            //
            // 初始化
            ViewModelUnitTestArchitecture.Initialize(Xaml.Container);
        }
        
        [TestMethod]
        public void Initialize()
        {
            // Tester.Run<ApplicationUnitTest>();
        }
    }
    
    public static class ViewModelUnitTestArchitecture
    {
        private static ILogger ConfigureLogger()
        {
            var config = new LoggingConfiguration();
            var debugFileTarget = new DebuggerTarget
            {
                Layout = "【${level}】 ${date:HH:mm:ss} ${message}"
            };

            var logfile = new FileTarget("logfile")
            {
                FileName = "${shortdate}.log",
                Layout   = "${level} ${shortdate} | ${message}  ${exception} ${event-properties:myProperty}"
            };

            config.AddRule(LogLevel.Warn, LogLevel.Fatal, logfile);
            config.AddRule(LogLevel.Info, LogLevel.Fatal, debugFileTarget);

            LogManager.Configuration = config;
            return LogManager.GetLogger("App");
        }

        public static void Initialize(IContainer container)
        {
            var logger = ConfigureLogger();


            //
            // 注册服务
            container.Use<ForestResourceFactory, ITextResourceFactory>(new ForestResourceFactory());
            container.Use<WindowEventBroadcast, IWindowEventBroadcast, IWindowEventBroadcastAmbient>(new WindowEventBroadcast());
            container.Use<DialogService,
                IDialogService,
                IDialogServiceAmbient,
                IBusyService,
                IBusyServiceAmbient,
                INotifyServiceAmbient,
                INotifyService,
                IBuiltinDialogService>(new DialogService());
            container.RegisterInstance<IScheduler>(CurrentThreadScheduler.Instance);
            container.RegisterInstance<ILogger>(logger);
            var dbMgr = container.Use<DatabaseManager, IDatabaseManager>(
                DatabaseManager.GetDefaultDatabaseManager(logger));

            dbMgr.LoadAsync("C:\\")
                 .GetAwaiter()
                 .GetResult();
            Xaml.Use<BasicAppSetting>(new BasicAppSetting());
            Xaml.Use<AutoSaveService, IAutoSaveService>(new AutoSaveService());
        }
        
    }
}