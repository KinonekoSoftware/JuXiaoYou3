using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Styles;

namespace Acorisoft.FutureGL.Tools.MusicPlayer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            ThemeSystem.Instance.Theme = new ForestLightTheme();
            Xaml.Use<WindowEventBroadcast, IWindowEventBroadcast, IWindowEventBroadcastAmbient>(new WindowEventBroadcast());
        }
    }
}