using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.UI;
using DryIoc;
using NLog;

namespace StoryDiagram
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

        protected override void RegisterResourceDictionary(ResourceDictionary appResDict)
        {
            appResDict.MergedDictionaries.Add(ForestUI.UseToolKits());
        }
    }
}