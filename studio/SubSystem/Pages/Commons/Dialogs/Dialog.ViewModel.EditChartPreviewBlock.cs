using System.Linq;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Presentations;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public class EditChartPresentationViewModel : ImplicitDialogVM
    {
        public static Task<Op<object>> Edit(ChartPresentation cb, DataPartCollection dataPartCollection)
        {
            var chartParts = dataPartCollection.OfType<PartOfModule>()
                                               .SelectMany(x => x.Blocks)
                                               .Where(x => x.ExtractType == cb.Type)
                                               .ToArray();
            return Xaml.Get<IDialogService>()
                       .Dialog(new EditChartPresentationViewModel(), new Parameter
                       {
                           Args = new object[]
                           {
                               cb,
                               chartParts
                           }
                       });
        }

        public EditChartPresentationViewModel()
        {
            Modules = new ObservableCollection<ModuleBlock>();
        }

        protected override void OnStart(RoutingEventArgs parameter)
        {
            var p                 = parameter.Parameter;
            var a                 = p.Args;
            Presentation = a[0] as ChartPresentation;
            var parts             = a[1] as ModuleBlock[];

            if (parts?.Length  > 0)
            {
                Modules.AddMany(parts, true);
            }
        }

        protected override bool IsCompleted() => Selected is not null;

        protected override void Finish()
        {
            var useMetadata = !string.IsNullOrEmpty(Selected.Metadata);
            Presentation.ValueSourceID = useMetadata ? Selected.Metadata : Selected.Id;
            Presentation.IsMetadata    = useMetadata;
        }

        protected override string Failed() => SubSystemString.Unknown;

        private ModuleBlock _selected;

        /// <summary>
        /// 获取或设置 <see cref="Selected"/> 属性。
        /// </summary>
        public ModuleBlock Selected
        {
            get => _selected;
            set => SetValue(ref _selected, value);
        }
        public ObservableCollection<ModuleBlock> Modules { get; }
        public ChartPresentation Presentation { get; private set; }
    }
}