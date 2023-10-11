using System.IO;
using System.Reactive.Concurrency;
using System.Threading;
using System.Windows.Threading;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Controls;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Utils;
using Acorisoft.FutureGL.Forest.Views;
using DryIoc;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Acorisoft.FutureGL.Forest
{
    public abstract class ForestApp : Application
    {
        protected class BuiltinViews : IBindingInfoProvider
        {
            public IEnumerable<BindingInfo> GetBindingInfo()
            {
                return new[]
                {
                    new BindingInfo
                    {
                        ViewModel = typeof(DangerViewModel),
                        View      = typeof(DangerView)
                    },
                    new BindingInfo
                    {
                        ViewModel = typeof(WarningViewModel),
                        View      = typeof(WarningView)
                    },
                    new BindingInfo
                    {
                        ViewModel = typeof(SuccessViewModel),
                        View      = typeof(SuccessView)
                    },
                    new BindingInfo
                    {
                        ViewModel = typeof(InfoViewModel),
                        View      = typeof(InfoView)
                    },
                    new BindingInfo
                    {
                        ViewModel = typeof(ObsoleteViewModel),
                        View      = typeof(ObsoleteView)
                    },
                    new BindingInfo
                    {
                        ViewModel = typeof(SingleLineViewModel),
                        View      = typeof(SingleLineView)
                    },
                    new BindingInfo
                    {
                        ViewModel = typeof(DangerNotifyViewModel),
                        View      = typeof(DangerNotifyView)
                    },
                    new BindingInfo
                    {
                        ViewModel = typeof(WarningNotifyViewModel),
                        View      = typeof(WarningNotifyView)
                    },
                    new BindingInfo
                    {
                        ViewModel = typeof(MultiLineViewModel),
                        View      = typeof(MultiLineView)
                    },
                };
            }
        }

        protected const  string BasicSettingDefaultFileName = "main.json";
        private readonly string BasicSettingFileName;
        private readonly bool   EnableDefaultSetting;

        protected ForestApp()
        {
            EnableDefaultSetting = false;

            //
            // 初始化
            Initialize();
        }

        protected ForestApp(string fileName)
        {
            EnableDefaultSetting = true;
            BasicSettingFileName = fileName;

            //
            // 初始化
            Initialize();
        }

        #region OnUnhandledException Handlers

        


        private void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
#if RELEASE
            e.Handled = true;
#endif
            Logger.Error(e.Exception);
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Error(e.ExceptionObject);
        }
        
        #endregion

        protected static void InstallView<TView, TViewModel>() where TView : ForestUserControl where TViewModel : ViewModelBase, IViewModel
        {
            Xaml.InstallView(new BindingInfo
            {
                ViewModel = typeof(TViewModel),
                View      = typeof(TView)
            });
        }

        private void Initialize()
        {
            
            Current.DispatcherUnhandledException       += OnUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            var (logger, appModel)                     =  RegisterFrameworkServicesIntern(Xaml.Container);
            RegisterServices(logger, appModel, Xaml.Container);
            Xaml.InstallViewFromSourceGenerator();
            RegisterViews(logger, Xaml.Container);
        }


        /*
         * 生命周期:
         *            sCtor
         *              |
         *             Ctor
         *              |
         *  RegisterFrameworkServices
         *              |
         *     RegisterServices (Internal)
         *              |
         *     RegisterServices (Public)
         *              |
         *         RegisterViews
         *              |
         *         ViewModel.Start
         *              |
         *  RegisterDependentServices
         *              |
         *          SplashView
         *              |
         *          OnStartUp
         *              |
         *           OnExit
         */

        #region Lifetime

        #region RegisterFrameworkServices

        #region Configure Methods

        /// <summary>
        /// 获得主题
        /// </summary>
        /// <returns>返回主题。</returns>
        /// <remarks>必须在启动前完成。</remarks>
        protected virtual ForestThemeSystem GetThemeSystem(BasicAppSetting basicAppSetting)
        {
            return basicAppSetting.Theme == MainTheme.Light ? new ForestLightTheme() : new ForestDarkTheme();
        }

        protected virtual string ConfigureLanguageFile()
        {
            var fileName = Language.Culture switch
            {
                CultureArea.English  => "en.ini",
                CultureArea.French   => "fr.ini",
                CultureArea.Japanese => "jp.ini",
                CultureArea.Korean   => "kr.ini",
                CultureArea.Russian  => "ru.ini",
                _                    => "cn.ini",
            };

            return Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Languages"), fileName);
        }


        /// <summary>
        /// 配置日志工具
        /// </summary>
        /// <param name="appModel">app模型</param>
        /// <returns>返回日志工具</returns>
        protected virtual ILogger ConfigureLogger(ApplicationModel appModel)
        {
            var config = new LoggingConfiguration();
            var debugFileTarget = new DebuggerTarget
            {
                Layout = "【${level}】 ${date:HH\\:mm\\:ss} ${message}"
            };

            var logfile = new FileTarget("logfile")
            {
                FileName = appModel.Logs + "/${shortdate}.log",
                Layout   = "${level} ${date:HH\\:mm\\:ss} | ${message}  ${exception} ${event-properties:myProperty}"
            };

            config.AddRule(LogLevel.Warn, LogLevel.Fatal, logfile);
            config.AddRule(LogLevel.Info, LogLevel.Fatal, debugFileTarget);

            LogManager.Configuration = config;
            return LogManager.GetLogger("App");
        }

        /// <summary>
        /// 配置目录
        /// </summary>
        protected virtual ApplicationModel ConfigureDirectory()
        {
            if (EnableDefaultSetting)
            {
                var domain = ApplicationModel.CheckDirectory(
                    Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                        "Forest"));

                return new ApplicationModel
                {
                    Logs     = Path.Combine(domain, "Logs"),
                    Settings = Path.Combine(domain, "UserData"),
                    Feedbacks = Path.Combine(domain, "Feedbacks"),
                }.Initialize();
            }
            else
            {
                var domain = AppDomain.CurrentDomain.BaseDirectory;
                return new ApplicationModel
                {
                    Logs      = Path.Combine(domain, "Logs"),
                    Settings  = Path.Combine(domain, "UserData"),
                    Feedbacks = Path.Combine(domain, "Feedbacks"),
                }.Initialize();
            }
        }

        #endregion

        /// <summary>
        /// 注册框架服务。
        /// </summary>
        /// <param name="container">服务容器。</param>
        private (ILogger, ApplicationModel) RegisterFrameworkServicesIntern(IContainer container)
        {
            var appModel = ConfigureDirectory();
            var logger   = ConfigureLogger(appModel);

            //
            //
            Logger   = logger;
            AppModel = appModel;

            BasicAppSetting basicAppSetting;

            //
            // 构建基本的属性
            if (EnableDefaultSetting)
            {
                basicAppSetting = JSON.OpenSetting<BasicAppSetting>(
                Path.Combine(appModel.Settings, BasicSettingFileName),
                () => new BasicAppSetting
                {
                    Language = CultureArea.Chinese,
                    Theme    = MainTheme.Light
                });
                
            }
            else
            {
                basicAppSetting = new BasicAppSetting
                {
                    Language = CultureArea.Chinese,
                    Theme    = MainTheme.Dark
                };
            }

            //
            // 设置主题。
            var theme = GetThemeSystem(basicAppSetting);
            theme.Initialize();
            ThemeSystem.Instance.Theme = theme;

            //
            // 设置语言。
            Language.Culture = basicAppSetting.Language;
            Language.SetLanguageSource(ConfigureLanguageFile());

            //
            // 注册服务
            container.Use<BasicAppSetting>(basicAppSetting);
            container.Use<ApplicationModel>(appModel);
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
            container.Use<IPendingQueue>(new PendingQueue());
            container.Use<ILogger>(logger);
            Xaml.InstallViewManually(new BuiltinViews());

            RegisterFrameworkServices(logger, appModel, container);
            return new ValueTuple<ILogger, ApplicationModel>(logger, appModel);
        }

        protected virtual void RegisterFrameworkServices(ILogger logger, ApplicationModel appModel, IContainer container)
        {
        }

        #endregion


        /// <summary>
        /// 注册服务。
        /// </summary>
        /// <param name="logger">日志工具。</param>
        /// <param name="appModel">应用模型。</param>
        /// <param name="container">服务容器。</param>
        protected abstract void RegisterServices(ILogger logger, ApplicationModel appModel, IContainer container);

        /// <summary>
        /// 注册视图
        /// </summary>
        /// <param name="logger">日志工具。</param>
        /// <param name="container">服务容器。</param>
        protected abstract void RegisterViews(ILogger logger, IContainer container);

        #region RegisterContextServices

        /// <summary>
        /// 注册上下文依赖的服务。
        /// </summary>
        internal void RegisterContextServicesIntern()
        {
            var container = Xaml.Container;
            container.Use<SynchronizationContextScheduler, IScheduler>(
                new SynchronizationContextScheduler(
                    SynchronizationContext.Current!));

            RegisterContextServices(Logger, AppModel, container);
        }

        protected virtual void RegisterContextServices(ILogger logger, ApplicationModel appModel, IContainer container)
        {
        }

        #endregion

        #region OnStartup

        protected sealed override void OnStartup(StartupEventArgs e)
        {
            RegisterResourceDictionary(Resources);
            StartupOverride(e);
            base.OnStartup(e);
        }

        protected virtual void StartupOverride(StartupEventArgs e)
        {
        }

        protected virtual void RegisterResourceDictionary(ResourceDictionary appResDict)
        {
        }

        #endregion


        #region OnExit

        protected sealed override void OnExit(ExitEventArgs e)
        {
            //
            // 重载
            OnExitOverride(e);

            //
            // 注意，当容器释放之后，程序将不可恢复。
            Xaml.Container
                .Dispose();

            base.OnExit(e);
        }

        protected virtual void OnExitOverride(ExitEventArgs e)
        {
        }

        #endregion

        #endregion
        
        private void SaveBasicSettingImpl(BasicAppSetting setting)
        {
            JSON.WriteSetting(Path.Combine(AppModel.Settings, BasicSettingFileName), setting);
        }
        
        /// <summary>
        /// 保存基础设置
        /// </summary>
        /// <param name="setting">设置</param>
        public static void SaveBasicSetting(BasicAppSetting setting) => ((ForestApp)Current).SaveBasicSettingImpl(setting);

        public ILogger Logger { get; private set; }
        public ApplicationModel AppModel { get; private set; }
    }
}