using System.Threading.Tasks;
using System.Windows.Controls;
using Acorisoft.FutureGL.Forest.Attributes;

namespace Acorisoft.FutureGL.Forest.Views
{
    [Connected(View = typeof(MultiLineView), ViewModel = typeof(MultiLineViewModel))]
    public partial class MultiLineView
    {
        public MultiLineView()
        {
            InitializeComponent();
        }
    }

    public sealed class MultiLineViewModel : ImplicitDialogVM
    {
        private string _text;

        protected override void OnStart(RoutingEventArgs parameter)
        {
            var param = parameter.Parameter
                                 .Args;
            Title = param[0]?.ToString();

            if (param.Length > 1)
            {
                Text = param[1]?.ToString();
            }

            base.OnStart(parameter);
        }

        protected override bool IsCompleted() => !string.IsNullOrEmpty(_text);

        protected override void Finish()
        {
            Result = _text;
        }

        protected override string Failed()
        {
            return "值为空";
        }

        /// <summary>
        /// 获取或设置 <see cref="Text"/> 属性。
        /// </summary>
        public string Text
        {
            get => _text;
            set
            {
                SetValue(ref _text, value);
            }
        }
        
        public static Task<Op<string>> String(string title) => DialogService()
            .Dialog<string>(new MultiLineViewModel(), new Parameter
            {
                Args = new object[]{ title }
            });
        
        public static Task<Op<string>> String(string title, string value) => DialogService()
            .Dialog<string>(new MultiLineViewModel(), new Parameter
            {
                Args = new object[]{ title, value }
            });
    }
}