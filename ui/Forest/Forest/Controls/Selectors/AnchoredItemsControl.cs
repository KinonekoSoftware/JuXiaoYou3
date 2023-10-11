using System.Linq;
using System.Windows.Controls.Primitives;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.Forest.Controls.Selectors
{
    public abstract class AnchoredItemsControl : ForestItemsControlBase
    {
        public class AnchoredItemInfo
        {
            public FrameworkElement Container { get; set; }
            public object DataContext { get; set; }
            public double Height { get; set; }
            public double Y { get; set; }
        }

        private readonly Point                  ZeroPoint          = new Point();
        private readonly List<AnchoredItemInfo> _anchoredItemInfos = new List<AnchoredItemInfo>(32);

        private bool _isAnchoredItemInfoGenerated;

        public AnchoredItemsControl()
        {
            ItemContainerGenerator.StatusChanged += OnItemsChanged;
        }


        public override void OnApplyTemplate()
        {
            ScrollViewer = (ScrollViewer)GetTemplateChild("PART_ScrollViewer");

            if (ScrollViewer is null)
            {
                return;
            }

            ScrollViewer.ScrollChanged          += OnScrollChanged;
            base.OnApplyTemplate();
        }

        private void OnItemsChanged(object sender, EventArgs e)
        {
            var i = ItemContainerGenerator.Items;

            if (ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
            {
                return;
            }

            _anchoredItemInfos.Clear();
            _isAnchoredItemInfoGenerated = false;

            i.Where(IsAnchorNode)
             .Select(x => new AnchoredItemInfo
             {
                 DataContext = x
             })
             .ForEach(x =>
             {
                 var container = ItemContainerGenerator.ContainerFromItem(x.DataContext);
                 x.Container = (FrameworkElement)container;
                 _anchoredItemInfos.Add(x);
             });
        }

        private static AnchoredItemInfo FindOutNearedAnchor(IReadOnlyList<AnchoredItemInfo> containers, double verticalOffset, double verticalHeight)
        {
            var hasItem  = false;
            var item     = (AnchoredItemInfo)null;
            var vh       = verticalOffset + verticalHeight;
            var distance = 1000000d;

            foreach (var container in containers)
            {
                if (container.Y >= verticalOffset &&
                    container.Y <= vh)
                {
                    hasItem = true;
                    var d = container.Y - verticalOffset;

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
                if (container.Y <= verticalOffset)
                {
                    var d = verticalOffset - container.Y;

                    if (d > 0 && d < distance)
                    {
                        distance = d;
                        item     = container;
                    }
                }
            }

            return item;
        }

        private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (!_isAnchoredItemInfoGenerated)
            {
                foreach (var info in _anchoredItemInfos)
                {
                    var f = info.Container;
                    if (f is null)
                    {
                        var container = ItemContainerGenerator.ContainerFromItem(info.DataContext);
                        info.Container = (FrameworkElement)container;
                    }

                    if (f is null)
                    {
                        continue;
                    }
                    var p = f.TranslatePoint(ZeroPoint, ScrollViewer);
                    info.Y      = p.Y;
                    info.Height = f.DesiredSize.Height;
                }

                _isAnchoredItemInfoGenerated = true;
            }

            if(ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
            {
                return;
            }

            var anchoredItem = FindOutNearedAnchor(_anchoredItemInfos, e.VerticalOffset, e.ViewportHeight);

            if (anchoredItem is null)
            {
                return;
            }

            OnAnchoredItemChanged(anchoredItem.DataContext);
        }

        protected abstract bool IsAnchorNode(object dataContext);
        protected abstract void OnAnchoredItemChanged(object item);

        public ScrollViewer ScrollViewer { get; private set; }
    }

    public abstract class AnchoredItemsControl<TDataContext, TAnchor> : AnchoredItemsControl
        where TDataContext : new()
        where TAnchor : TDataContext
    {
        public static readonly DependencyProperty AnchoredItemProperty = DependencyProperty.Register(
            nameof(AnchoredItem),
            typeof(object),
            typeof(AnchoredItemsControl<TDataContext, TAnchor>),
            new PropertyMetadata(default(object)));

        protected override void OnAnchoredItemChanged(object item)
        {
            AnchoredItem = (TAnchor)item;
        }

        protected override bool IsAnchorNode(object dataContext)
        {
            return dataContext is TAnchor;
        }

        public TAnchor AnchoredItem
        {
            get => (TAnchor)GetValue(AnchoredItemProperty);
            set => SetValue(AnchoredItemProperty, value);
        }
    }
}