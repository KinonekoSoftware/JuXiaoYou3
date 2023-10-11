using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.Forest.Interfaces
{
    public interface IInputViewModel : IDialogViewModel
    {
        /// <summary>
        /// 完成命令。
        /// </summary>
        RelayCommand CompletedCommand { get; }
        
        /// <summary>
        /// 结果
        /// </summary>
        object Result { get; }
    }
}