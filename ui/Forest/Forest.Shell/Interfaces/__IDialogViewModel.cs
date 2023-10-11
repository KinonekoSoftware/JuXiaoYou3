using DialogTaskSource = System.Threading.Tasks.TaskCompletionSource<Acorisoft.FutureGL.Forest.Op<object>>;

namespace Acorisoft.FutureGL.Forest.Interfaces
{
    internal interface __IDialogViewModel
    {
        DialogTaskSource WaitHandle { get; }
        
        Action CloseHandler { get; set; }
    }
}