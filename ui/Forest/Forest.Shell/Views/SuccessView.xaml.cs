using Acorisoft.FutureGL.Forest.Controls;

namespace Acorisoft.FutureGL.Forest.Views
{
    public partial class SuccessView:ForestUserControl 
    {
        public SuccessView()
        {
            InitializeComponent();
        }
    }
    
    
    public sealed class SuccessViewModel : BooleanDialogVM
    {
        private string _content;

        /// <summary>
        /// 获取或设置 <see cref="Content"/> 属性。
        /// </summary>
        public string Content
        {
            get => _content;
            set => SetValue(ref _content, value);
        }
    }
}