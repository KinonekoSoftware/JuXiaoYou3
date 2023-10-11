using System.Windows;
using System.Windows.Controls;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.Forest.ViewModels;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.ViewModels;
using DryIoc;
using NLog;

namespace Acorisoft.FutureGL.MigaStudio.Windows
{
    public abstract class TabApp<TViewModel, TMainController, TMainView> : ForestApp
        where TMainController : ITabViewController
        where TMainView : UserControl
        where TViewModel : TabBaseAppViewModel
    {
        protected TabApp()
        {
        }

        protected TabApp(string fileName) : base(fileName)
        {
        }

        protected override void RegisterFrameworkServices(ILogger logger, ApplicationModel appModel, IContainer container)
        {
            Xaml.InstallView(new BindingInfo
            {
                View      = typeof(TMainView),
                ViewModel = typeof(TMainController)
            });


            container.Use<ViewServiceAdapter,IViewServiceAmbient>(new ViewServiceAdapter());

            //
            // 创建Shell
            Xaml.Use<TViewModel, TabBaseAppViewModel, AppViewModelBase>(GetShell());
        }

        protected override void RegisterResourceDictionary(ResourceDictionary appResDict)
        {
            appResDict.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/Forest.Controls;component/Themes/Generic.xaml", UriKind.RelativeOrAbsolute)
            });

            appResDict.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/Forest.Shell;component/Themes/Generic.xaml", UriKind.RelativeOrAbsolute)
            });

            appResDict.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/Forest.Fonts;component/Fonts.xaml", UriKind.RelativeOrAbsolute)
            });
        }

        protected abstract TViewModel GetShell();
    }

    public abstract class TabApp<TViewModel, TMainController, TMainView, TSplashController, TSplash> : TabApp<TViewModel, TMainController, TMainView>
        where TMainController : ITabViewController
        where TMainView : UserControl
        where TSplashController : IRootViewModel
        where TSplash : UserControl
        where TViewModel : TabBaseAppViewModel
    {
        protected TabApp() : base(BasicSettingDefaultFileName)
        {
        }

        protected TabApp(string fileName) : base(fileName)
        {
        }

        protected override void RegisterFrameworkServices(ILogger logger, ApplicationModel appModel, IContainer container)
        {
            Xaml.InstallView(new BindingInfo
            {
                View      = typeof(TSplash),
                ViewModel = typeof(TSplashController)
            });
            base.RegisterFrameworkServices(logger, appModel, container);
        }
    }
}