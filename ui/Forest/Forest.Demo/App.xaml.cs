using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Services;
using Acorisoft.FutureGL.Forest.Styles;
using Acorisoft.FutureGL.Forest.ViewModels;
using NLog;
using IContainer = DryIoc.IContainer;

namespace Acorisoft.FutureGL.Forest
{
    
    public abstract class Trackable
    {
        public string Id { get; init; }
        public string Name { get; set; }
        public string DataID { get; set; }
        public string EngineID { get; set; }
        public string Hash { get; set; }
        public DateTime TimeOfCreated { get; set; }
        public DateTime TimeOfModified { get; set; }
        public DateTime TimeOfSync { get; set; }
        public bool IsDeleted { get; set; }
    }

    public abstract class DiscreteTrackable : Trackable, INotifyPropertyChanged
    {
        
    }


    public abstract class QuTrackable : Trackable, INotifyPropertyChanged
    {
        
    }
    
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void RegisterServices(ILogger logger, IContainer container)
        {
            
        }

        protected override void RegisterViews(ILogger logger, IContainer container)
        {
        }
    }
}