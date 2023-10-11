using System.Linq;
using Acorisoft.FutureGL.Forest.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public class ModuleSelectorViewModel : ImplicitDialogVM
    {
        protected override bool IsCompleted() => true;

        protected override void OnStart(RoutingEventArgs parameter)
        {
            var array = parameter.Parameter.Args[0] as IEnumerable<ModuleTemplateCache>;
            Templates.AddMany(array, true);
        }

        protected override void Finish()
        {
            if (TargetElement is null)
            {
                return;
            }

            Result = TargetElement.SelectedItems
                                  .Cast<ModuleTemplateCache>()
                                  .ToArray();
        }

        protected override string Failed() => SubSystemString.Unknown;

        private ListBox _targetElement;

        /// <summary>
        /// 获取或设置 <see cref="TargetElement"/> 属性。
        /// </summary>
        public ListBox TargetElement
        {
            get => _targetElement;
            set => SetValue(ref _targetElement, value);
        }

        public ObservableCollection<ModuleTemplateCache> Templates { get; } = new ObservableCollection<ModuleTemplateCache>();
    }
}