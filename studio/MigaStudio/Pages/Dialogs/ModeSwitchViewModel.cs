using System.Linq;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaUtils;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class ModeSwitchViewModel : ImplicitDialogVM
    {
        private string             _selectedController;
        private ControllerManifest _selected;

        public ModeSwitchViewModel()
        {
            Controllers = new ObservableCollection<ControllerManifest>();
        }

        protected override void OnStart(RoutingEventArgs parameter)
        {
            var p = parameter.Parameter;
            var a = p.Args;
            Context            = (GlobalStudioContext)a[0];
            Controllers.AddMany(Context.ControllerList, true);
            Selected = Controllers.FirstOrDefault(x => x.Value == Context.CurrentController.Id);
            base.OnStart(parameter);
        }

        public static Task<Op<object>> Switch(GlobalStudioContext context)
        {
            return DialogService().Dialog(new ModeSwitchViewModel(), new Parameter
            {
                Args = new object[] { context }
            });
        }

        protected override bool IsCompleted() => true;

        protected override void Finish()
        {
            if (string.IsNullOrEmpty(SelectedController))
            {
                return;
            }

            if (!Context.ControllerMaps
                        .TryGetValue(SelectedController, out var controller))
            {
                return;
            }
            
            Context.SwitchController(controller);
        }

        public static void SwitchTo(GlobalStudioContext context, ControllerManifest manifest)
        {
            if (context is null ||
                string.IsNullOrEmpty(manifest?.Value))
            {
                return;
            }

            if (!context.ControllerMaps
                        .TryGetValue(manifest.Value, out var controller))
            {
                return;
            }
            
            context.SwitchController(controller);
        }
        
        public static void SwitchTo(GlobalStudioContext context, string value)
        {
            
            if (context is null ||
                string.IsNullOrEmpty(value))
            {
                return;
            }

            if (!context.ControllerMaps
                        .TryGetValue(value, out var controller))
            {
                return;
            }
            
            context.SwitchController(controller);
        }

        protected override string Failed() => SubSystemString.Unknown;

        public GlobalStudioContext Context { get; private set; }

        public ObservableCollection<ControllerManifest> Controllers { get; }

        /// <summary>
        /// 获取或设置 <see cref="Selected"/> 属性。
        /// </summary>
        public ControllerManifest Selected
        {
            get => _selected;
            set
            {
                SetValue(ref _selected, value);
                SelectedController = value?.Value;
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="SelectedController"/> 属性。
        /// </summary>
        public string SelectedController
        {
            get => _selectedController;
            set => SetValue(ref _selectedController, value);
        }
    }
}