using System.Reactive.Subjects;
using System.Windows.Input;

namespace Acorisoft.FutureGL.Forest.Interfaces
{
    public interface IWindowEventBroadcast
    {
        /// <summary>
        /// 键盘事件
        /// </summary>
        IObservable<WindowKeyEventArgs> Keys { get; }
        
        /// <summary>
        /// 拖拽事件
        /// </summary>
        IObservable<WindowDragDropArgs> Drags { get; }
        
        /// <summary>
        /// 属性隧道
        /// </summary>
        IWindowPropertyTunnel PropertyTunnel { get; }
    }

    public interface IWindowEventBroadcastAmbient
    {
        void SetEventSource(Window window);
    }

    class WindowPropertyTunnel: IWindowPropertyTunnel
    {
        public Action<WindowState> WindowState { get; set; }
    }
    
    public class WindowEventBroadcast : IWindowEventBroadcast, IWindowEventBroadcastAmbient
    {
        private readonly Subject<WindowKeyEventArgs> _keys  = new Subject<WindowKeyEventArgs>();
        private readonly Subject<WindowDragDropArgs> _drags = new Subject<WindowDragDropArgs>();
        private readonly IWindowPropertyTunnel _tunnel = new WindowPropertyTunnel();

        private Window _eventSource;
        public void SetEventSource(Window window)
        {
            if (window is null)
            {
                return;
            }
            
            if (_eventSource is not null)
            {
                _eventSource.KeyUp     -= OnKeyDown;
                _eventSource.KeyUp     -= OnKeyUp;
            }

            _eventSource       =  window;
            _eventSource.KeyUp += OnKeyDown;
            _eventSource.KeyUp += OnKeyUp;
        }


        private void OnDragOver(object sender, DragEventArgs e)
        {
            _drags.OnNext(new WindowDragDropArgs
            {
                State = DragDropState.Dropped,
                Args  = e
            });
            e.Handled = true;
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            _drags.OnNext(new WindowDragDropArgs
            {
                State = DragDropState.DragStart,
                Args  = e
            });
            e.Handled = true;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {

        }
        
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if(e.IsRepeat || e.Key == Key.CapsLock) 
            { 
                return; 
            }
            _keys.OnNext(new WindowKeyEventArgs
            {
                Args   = e,
                IsDown = false,
            });
        }
        
        public IObservable<WindowKeyEventArgs> Keys => _keys;
        public IObservable<WindowDragDropArgs> Drags => _drags;
        public IWindowPropertyTunnel PropertyTunnel => _tunnel;
    }
}