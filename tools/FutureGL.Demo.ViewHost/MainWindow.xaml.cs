using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Interfaces;

namespace Acorisoft.FutureGL.Demo.ViewHost
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Xaml.Get<IWindowEventBroadcast>()
                .Keys
                .Subscribe(x =>
            {
                if (x.Args.Key == Key.F1)
                {
                    Drawer.IsLeftOpen = true;
                }
            });
            
            
        }

        public static int[] Array { get; } = Enumerable.Range(1, 300).ToArray();
    }
}