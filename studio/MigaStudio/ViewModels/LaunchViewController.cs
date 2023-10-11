using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data.Keywords;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaUtils;
using NLog;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public class LaunchViewController : LaunchViewControllerBase
    {
        private readonly AssemblyLoadContext _alc;

        public LaunchViewController()
        {
            _alc = AssemblyLoadContext.Default;

            // 加载设置
            Job("text.launch.loadSetting", LaunchSettingImpl);

            // 检查更新
            Job("text.launch.checkVersion", x => { });
            
            // 检查世界观存在性
            Job("text.launch.databaseExistence", x =>  CheckDatabaseExistenceImpl((GlobalStudioContext)x));

            // 加载插件
            Job("text.launch.loadModules", x => LoadModuleImpl((GlobalStudioContext)x));

            // 打开数据库
            Job("text.launch.openDatabase", x => OpenDatabaseImpl((GlobalStudioContext)x));
        }

        private static string GetPluginDirectory()
        {
            var pluginDir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
            if (!System.IO
                       .Directory
                       .Exists(pluginDir))
            {
                System.IO
                      .Directory
                      .CreateDirectory(pluginDir);
            }

            return pluginDir;
        }

        private static void LaunchSettingImpl(object p)
        {
            var gc      = (GlobalStudioContext)p;
            var ss      = Xaml.Get<SystemSetting>();
            var setting = ss.AdvancedSetting;
            var assemblyVersion = Assembly.GetAssembly(typeof(MainWindow))
                                          .GetCustomAttribute<AssemblyInformationalVersionAttribute>();

            var version = assemblyVersion?.InformationalVersion ?? "3.0.0";

            if (string.IsNullOrEmpty(setting.ApplicationVersion))
            {
                //
                //
                setting.ApplicationVersion = version;
                ss.Save();
                return;
            }

            if (setting.ApplicationVersion != version)
            {
                setting.DebugMode = DatabaseMode.Attached;
                gc.IsUpdated      = true;
                ss.Save();
            }
        }

        private void LoadModuleImpl(GlobalStudioContext context)
        {
            var pluginDir = GetPluginDirectory();
            var maybePluginFiles = System.IO
                                         .Directory
                                         .GetFiles(pluginDir, "*.dll");
            foreach (var maybePluginFile in maybePluginFiles)
            {
                try
                {
                    var assembly = _alc.LoadFromAssemblyPath(maybePluginFile);
                    var ssmType  = Classes.FindInterfaceImplementation<ISubSystemModule>(assembly);

                    if (ssmType is null)
                    {
                        Xaml.Get<ILogger>()
                            .Warn($"文件：{maybePluginFile}是可识别的插件，但缺少默认的插件实现！");
                        continue;
                    }

                    var ssm = (ISubSystemModule)Activator.CreateInstance(ssmType);

                    if (ssm is null)
                    {
                        Xaml.Get<ILogger>()
                            .Warn($"文件：{maybePluginFile}是可识别的插件，但无法创建插件实现！");
                        continue;
                    }

                    //
                    // 注册
                    ssm.Initialize(Language.Culture, Xaml.Container);

                    var nameItem = new ControllerManifest
                    {
                        Name  = ssm.FriendlyName,
                        Value = ssm.ModuleId,
                        Intro = ssm.Intro
                    };

                    //
                    // 添加控制器列表
                    context.ControllerList.Add(nameItem);

                    //
                    // 添加控制器映射
                    var map = (Dictionary<string, ITabViewController>)context.ControllerMaps;
                    if (map.TryAdd(ssm.ModuleId, ssm.GetController()))
                    {
                        Xaml.Get<ILogger>()
                            .Info($"已加载插件，插件ID:{ssm.ModuleId}, 插件名:{ssm.FriendlyName}, 路径:{maybePluginFile}！");
                    }
                    else
                    {
                        Xaml.Get<ILogger>()
                            .Warn($"无法加载插件，已有同名插件，插件ID:{ssm.ModuleId}, 插件名:{ssm.FriendlyName}, 路径:{maybePluginFile}！");
                    }
                }
                catch
                {
                    Xaml.Get<ILogger>()
                        .Warn($"文件：{maybePluginFile}并不是可识别的插件！");
                }
            }
        }

        private static void OpenDatabaseImpl(GlobalStudioContext context)
        {
            var setting = Xaml.Get<SystemSetting>()
                              .RepositorySetting;

            if (string.IsNullOrEmpty(setting.LastRepository))
            {
                return;
            }

            var dr = Studio.DatabaseManager()
                           .LoadAsync(setting.LastRepository, Xaml.Get<SystemSetting>()
                                                                  .AdvancedSetting
                                                                  .DebugMode)
                           .GetAwaiter()
                           .GetResult();

            if (dr.IsFinished)
            {
                context.IsDatabaseOpen = true;
            }
            else
            {
                Xaml.Get<IDialogService>()
                    .Notify(
                        CriticalLevel.Warning,
                        Language.NotifyText,
                        SubSystemString.GetDatabaseResult(dr.Reason));
            }
        }

        private static void CheckDatabaseExistenceImpl(GlobalStudioContext _)
        {
            var ss = Xaml.Get<SystemSetting>();
            var rs = ss.RepositorySetting;

            for (var i = 0; i < rs.Repositories.Count; i++)
            {
                if (System.IO.Directory.Exists(rs.Repositories[i].Path))
                {
                    continue;
                }
                
                rs.Repositories
                  .RemoveAt(i);
            }
        }

        protected override void OnStartup(RoutingEventArgs arg)
        {
            Context = arg.Parameter.Args[0] as GlobalStudioContext;
            Init();
        }

        protected override object GetExecuteContext() => Context;

        protected override void OnJobCompleted()
        {
            var opening = Context.IsDatabaseOpen;

            //
            //
            Context.SwitchController(AppViewModel.Route(Context, opening));
        }

        public GlobalStudioContext Context { get; private set; }
    }
}