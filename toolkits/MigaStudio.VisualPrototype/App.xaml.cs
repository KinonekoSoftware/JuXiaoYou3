using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.Forest.Styles;
using DryIoc;
using NLog;

namespace Acorisoft.FutureGL.MigaStudio.Repairs
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void RegisterServices(ILogger logger, ApplicationModel appModel, IContainer container)
        {
            
        }

        protected override void RegisterViews(ILogger logger, IContainer container)
        {
        }
    }
}