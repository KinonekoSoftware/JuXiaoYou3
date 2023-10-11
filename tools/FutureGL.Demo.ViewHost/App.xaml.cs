using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.Forest.Services;
using Acorisoft.FutureGL.Forest.Styles;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaStudio.Pages;
using Acorisoft.FutureGL.MigaStudio.Pages.Gallery;
using DryIoc;

namespace Acorisoft.FutureGL.Demo.ViewHost
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App() 
        {
            Language.SetLanguageSource("cn.ini");
        }
        
        private IDatabaseManager _databaseManager;

        protected override void RegisterFrameworkServices(ILogger logger, ApplicationModel appModel, IContainer container)
        {
            container.Use<ViewService, IViewService, IViewServiceAmbient>(new ViewService());
        }

        protected override void RegisterServices(ILogger logger, ApplicationModel appModel, IContainer container)
        {
            _databaseManager = container.Use<DatabaseManager, IDatabaseManager>(
                DatabaseManager.GetDefaultDatabaseManager(logger));
        }

        protected override void RegisterViews(ILogger logger, IContainer container)
        {
            SubSystem.InstallViews();
        }


        protected override async void OnExitOverride(ExitEventArgs e)
        {
            //
            // 移除所有对象
            await _databaseManager.CloseAsync();
        }
    }
}