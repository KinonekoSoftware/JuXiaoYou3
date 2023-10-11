using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Controls;

namespace Acorisoft.FutureGL.Forest.Services
{
    public class BusyService : IBusyService, IBusyServiceAmbient
    {
        class Session : IBusySession
        {
            private void RunOnDispatcherThread(Action action)
            {
                var dispatcher = Host.Dispatcher;
                if (dispatcher.CheckAccess())
                    action();
                else
                    dispatcher.Invoke(action);
            }
            
            
            public Task Await() => Task.Delay(100);


            public Task Await(string text)
            {
                Update(text);
                return Task.Delay(100);  
            } 
            
            public void Dispose()
            {
                if (Host is null)
                {
                    return;
                }
                
                RunOnDispatcherThread(() =>
                {
                    Host.IsBusy = false;
                });

                Clean();
            }

            public void Update(string text)
            {
                if (Host is null)
                {
                    return;
                }

                RunOnDispatcherThread(() =>
                {

                    //
                    // 更新
                    Host.BusyText = text;

                    if (!Host.IsBusy)
                    {
                        Host.IsBusy = true;
                    }
                });
            }
            
            public DialogHost Host { get; init; }
            public Action Clean { get; init; }
        }

        private DialogHost _host;
        private Session _session;
        
        public IBusySession CreateSession()
        {
            _session ??= new Session
            {
                Host  = _host,
                Clean = () => _session = null,
            };


            return _session;
        }

        /// <summary>
        /// 设置服务响应者
        /// </summary>
        /// <param name="host"></param>
        public void SetServiceProvider(DialogHost host) => _host = host;
    }
}