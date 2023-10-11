using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.UI;
using Acorisoft.FutureGL.Forest.Utils;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.Pages;
using Acorisoft.FutureGL.MigaStudio.Services;
using DryIoc;
using NLog;

namespace Acorisoft.FutureGL.MigaStudio
{
    partial class App
    {
        protected override void RegisterServices(ILogger logger, ApplicationModel appModel, IContainer container)
        {
            var setting = InstallSetting(logger, appModel, container);
            _databaseManager = container.Use<DatabaseManager, IDatabaseManager>(DatabaseManager.GetDefaultDatabaseManager(logger));
            var attachable = new InMemoryServiceHost(container, _databaseManager);

            //
            // 注册数据库附加服务
            InstallInMemoryService(attachable);
            InstallAutoSaveService(logger, setting, container);

            container.RegisterInstance<MusicService>(new MusicService());
        }

        private static void InstallAutoSaveService(ILogger logger, AdvancedSettingModel setting, IContainer container)
        {
            var autoSave = new AutoSaveService();
            var period   = Math.Clamp(setting.AutoSavePeriod, 1, 10);
            autoSave.Elapsed = period;
            logger.Info($"设置自动保存服务，自动保存时间为：{period}");


            //
            // 注册服务
            container.Use<AutoSaveService, IAutoSaveService>(autoSave);
        }

        private static AdvancedSettingModel InstallSetting(ILogger logger, ApplicationModel appModel, IContainer container)
        {
            logger.Info("正在读取设置...");

            //
            // Repository Setting
            var repositorySettingFileName = Path.Combine(appModel.Settings, RepositorySettingFileName);
            var repositorySetting = JSON.OpenSetting<RepositorySetting>(repositorySettingFileName,
                () => new RepositorySetting
                {
                    LastRepository = null,
                    Repositories   = new ObservableCollection<RepositoryCache>()
                });

            //
            // Advanced Setting
            var advancedSettingFileName = Path.Combine(appModel.Settings, AdvancedSettingFileName);
            var advancedSetting = JSON.OpenSetting<AdvancedSettingModel>(advancedSettingFileName,
                () => new AdvancedSettingModel
                {
                    DebugMode = DatabaseMode.Release,
                });

            //
            // 注册设置
            container.Use<SystemSetting, ISystemSetting>(new SystemSetting
            {
                AdvancedSettingFileName   = advancedSettingFileName,
                AdvancedSetting           = advancedSetting,
                RepositorySetting         = repositorySetting,
                RepositorySettingFileName = repositorySettingFileName
            });

            return advancedSetting;
        }


        public static string ApplicationAssemblyVersion
        {
            get
            {
                var prefix = Language.Culture switch
                {
                    CultureArea.English  => "Assembly Version",
                    CultureArea.Russian  => "Версия сборки",
                    CultureArea.French   => "Version de l’assemblage",
                    CultureArea.Korean   => "어셈블리 버전",
                    CultureArea.Japanese => "アセンブリ バージョン",
                    _                    => "程序集版本"
                };

                var version = Assembly.GetAssembly(typeof(App))
                                      .GetCustomAttribute<AssemblyVersionAttribute>()
                                      ?.Version ?? "3.0.0";

                return $"{prefix}:\t{version}";
            }
        }

        public static string ApplicationAssemblyFileVersion
        {
            get
            {
                var prefix = Language.Culture switch
                {
                    CultureArea.English  => "Insider Version",
                    CultureArea.Russian  => "Инсайдерская версия",
                    CultureArea.French   => "Insider Version",
                    CultureArea.Korean   => "참가자 버전",
                    CultureArea.Japanese => "インサイダーバージョン",
                    _                    => "内部开发版本号"
                };

                var version = Assembly.GetAssembly(typeof(App))!
                                      .GetName()
                                      .Version!
                                      .ToString();

                return $"{prefix}:\t{version}";
            }
        }

        public static string ApplicationVersion
        {
            get
            {
                var prefix = Language.Culture switch
                {
                    CultureArea.English  => "Application Version",
                    CultureArea.Russian  => "Версия приложения",
                    CultureArea.French   => "Version de l’application",
                    CultureArea.Korean   => "응용 프로그램 버전",
                    CultureArea.Japanese => "アプリケーションのバージョン",
                    _                    => "应用程序版本"
                };

                var version = Assembly.GetAssembly(typeof(App))
                                      .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                                      ?.InformationalVersion ?? "3.0.0";

                return $"{prefix}:\t{version}";
            }
        }
    }
}