using System.Collections.Generic;
using Acorisoft.FutureGL.MigaDB.Core;
using DryIoc;

namespace Acorisoft.FutureGL.MigaStudio.Core
{

    public interface IInMemoryServiceHost
    {
        void Add(IInMemoryDatabaseService service);
        void Remove(IInMemoryDatabaseService service);
    }

    public class InMemoryServiceHost : Disposable, IInMemoryServiceHost
    {
        private readonly IContainer                     _container;
        private readonly IDatabaseManager               _databaseManager;
        private readonly List<IInMemoryDatabaseService> _services;
        private readonly IDisposable                    _disposable;

        public InMemoryServiceHost(IContainer container, IDatabaseManager databaseManager)
        {
            _container       = container;
            _databaseManager = databaseManager;
            _services        = new List<IInMemoryDatabaseService>(16);
            _disposable = _databaseManager.IsOpen
                                          .Observable
                                          .Subscribe(OnDatabaseOpenStateChanged);
            
            container.Use<InMemoryServiceHost, IInMemoryServiceHost>(this);
        }

        protected override void ReleaseManagedResources()
        {
            _disposable.Dispose();
        }

        private void OnDatabaseOpenStateChanged(bool x)
        {
            if (x)
            {
                _services.ForEach(a => a.Start(_databaseManager));
            }
            else
            {
                _services.ForEach(a => a.Stop());
            }
        }

        public void Add(IInMemoryDatabaseService service)
        {
            if (service is null)
            {
                return;
            }
            service.Register(_container);
            _services.Add(service);
        }

        public void Remove(IInMemoryDatabaseService service)
        {
            if (service is null)
            {
                return;
            }
            
            service.Unregister(_container);
            _services.Remove(service);
        }
    }
}