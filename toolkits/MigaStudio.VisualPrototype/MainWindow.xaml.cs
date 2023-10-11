using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Acorisoft.FutureGL.Forest.Controls.Selectors;
using Acorisoft.FutureGL.Forest.ViewModels;
using Acorisoft.FutureGL.MigaUtils.Collections;
using ImTools;

namespace Acorisoft.FutureGL.MigaStudio.Repairs
{
    public class Number
    {
        public string Content { get; set; }
        public int Num { get; set; }
    }

    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            Items = new ObservableCollection<Number>(Enumerable.Range(1, 100)
                                                               .Select(x => new Number
                                                               {
                                                                   Content = x.ToString(),
                                                                   Num     = x
                                                               }));
        }

        private Number _anchor;

        /// <summary>
        /// 获取或设置 <see cref="Anchor"/> 属性。
        /// </summary>
        public Number Anchor
        {
            get => _anchor;
            set
            {
                SetValue(ref _anchor, value);
                Debug.WriteLine($"当前项: {value.Num}");
            }
        }

        public ObservableCollection<Number> Items { get; }
    }

    public class NumberItemsControl : AnchoredItemsControl<Number, Number>
    {
        protected override bool IsAnchorNode(object dataContext)
        {
            return dataContext is Number n && n.Num % 10 == 0;
        }
    }

    public class Container
    {
        public Number Number { get; set; }
        public object Owner { get; set; }
        public Point Point { get; set; }
        public double ControlBound { get; set; }
    }
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public partial class MainWindow
    {
        private          int                    _count;
        private readonly ObservableCollection<Container> _dictionary;
        private          bool                   _generated;
        
        public MainWindow()
        {
            _count      = 100;
            _dictionary = new ObservableCollection<Container>();
            InitializeComponent();
            
        }

        protected override void OnLoaded(object sender, RoutedEventArgs e)
        {
            base.OnLoaded(sender, e);
        }

        private void ItemContainerGeneratorOnStatusChanged(object sender, EventArgs e)
        {
            var i = ItemsControl.ItemContainerGenerator
                                .Items;

            if (ItemsControl.ItemContainerGenerator
                            .Status != GeneratorStatus.ContainersGenerated)
            {
                return;
            }
            
            _dictionary.Clear();
            _generated = false;

            i.OfType<Number>()
             .Where(x => x.Num % 10 == 0)
             .Select(x=> new Container
             {
                 Number = x
             })
             .ForEach(x =>
             {
                 var container = ItemsControl.ItemContainerGenerator
                                             .ContainerFromItem(x.Number);
                 x.Owner = container;
                 _dictionary.Add(x);
             });
            
            Debug.WriteLine("Regenerated Container Info");
        }

        private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (!_generated)
            {
                Debug.WriteLine("Generating Scroll Info");
                var zero = new Point();
            
                foreach (var container in _dictionary)
                {
                    var f = (FrameworkElement)container.Owner;
                    if (f is null)
                    {
                        continue;
                    }
                    var p  = f.TranslatePoint(zero, ItemsControl);
                    container.Point        = p;
                    container.ControlBound = p.Y + f.DesiredSize.Height;
                }
                
                _generated = true;
                Debug.WriteLine("Generated Scroll Info");
            }

            var min = FindOutNearedAnchor(_dictionary, e.VerticalOffset, e.ViewportHeight);

            if (min is null)
            {
                return;
            }
            
            Debug.WriteLine($"当前项：{min.Number.Num}");
        }

        private static Container FindOutNearedAnchor(IEnumerable<Container> containers, double verticalOffset, double verticalHeight)
        {
            var hasItem  = false;
            var item     = (Container)null;
            var vh       = verticalOffset + verticalHeight;
            var distance = 1000000d;

            foreach (var container in containers)
            {
                if (container.Point.Y >= verticalOffset &&
                    container.Point.Y <= vh)
                {
                    hasItem = true;
                    var d = container.Point.Y - verticalOffset;

                    if (d > 0 && d < distance)
                    {
                        distance = d;
                        item     = container;
                    }
                }
            }

            if (hasItem)
            {
                return item;
            }
            
            distance = 1000000d;
            foreach (var container in containers)
            {
                if (container.Point.Y <= verticalOffset)
                {
                    hasItem = true;
                    var d = verticalOffset - container.Point.Y;

                    if (d > 0 && d < distance)
                    {
                        distance = d;
                        item     = container;
                    }
                }
            }

            return item;
        }

        private void Window_AddNumber(object sender, MouseButtonEventArgs e)
        {
            ((MainWindowViewModel)DataContext).Items
                        .Add(new Number
                        {
                            Num     = _count,
                            Content = _count.ToString()
                        });
        }
    }
}