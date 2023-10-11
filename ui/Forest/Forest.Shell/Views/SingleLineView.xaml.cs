using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest.Controls;
using ImTools;

namespace Acorisoft.FutureGL.Forest.Views
{
    public partial class SingleLineView:ForestUserControl 
    {
        public SingleLineView()
        {
            InitializeComponent();
        }
    }

    public sealed class SingleLineViewModel : ImplicitDialogVM
    {
        private string     _text;

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
                                                       .Dialog<string>(new SingleLineViewModel(), new Parameter
                                                       {
                                                           Args = new object[]{ title }
                                                       });
        
        public static Task<Op<string>> String(string title, string value) => DialogService()
            .Dialog<string>(new SingleLineViewModel(), new Parameter
            {
                Args = new object[]{ title ,value }
            });
    }
}