using System.Reactive;
using System.Reactive.Subjects;
using System.Threading;

namespace Acorisoft.FutureGL.MigaStudio.Core
{
    public interface IAutoSaveService
    {
        IObservable<Unit> Observable { get; }
    }

    public class AutoSaveService : Disposable, IAutoSaveService
    {
        public const     int           MinutesToTick = 30;
        private readonly Subject<Unit> _subject;
        private readonly Timer         _timer;
        private          int           _secondCount;
        private          int           _triggerCount;
        
        public AutoSaveService()
        {
            _subject      = new Subject<Unit>();
            _triggerCount = 100;
            _timer        = new Timer(DurationPushHandler, null, 0, 2000);
        }

        protected override void ReleaseManagedResources()
        {
            _subject.Dispose();
            _timer.Dispose();
        }

        private void DurationPushHandler(object state)
        {
            _secondCount++;
            
            if (_secondCount >= _triggerCount)
            {
                _secondCount = 0;
                _subject.OnNext(Unit.Default);
            }
        }

        public int Elapsed
        {
            get => _triggerCount;
            set
            {
                var v = Math.Clamp(value, 1, 10);
                _triggerCount = v * MinutesToTick;
            }
        }

        public IObservable<Unit> Observable => _subject;
    }
}