using System.Linq;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public class OptionSelectionViewModel : ImplicitDialogVM
    {
        private object              _selected;
        private IEnumerable<object> _options;

        protected override void OnStart(RoutingEventArgs parameter)
        {
            var param = parameter.Parameter;
            _selected = param.Args[0];
            _options  = param.Args[1] as IEnumerable<object>;
        }

        protected override bool IsCompleted() => _options.Contains(_selected);

        protected override void Finish()
        {
            Result = _selected;
        }

        protected override string Failed() => "选择的内容不在当前选项集合！";

        /// <summary>
        /// 获取或设置 <see cref="Options"/> 属性。
        /// </summary>
        public IEnumerable<object> Options
        {
            get => _options;
            set => SetValue(ref _options, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Selected"/> 属性。
        /// </summary>
        public object Selected
        {
            get => _selected;
            set => SetValue(ref _selected, value);
        }
    }
}