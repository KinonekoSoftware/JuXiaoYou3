using System.Reactive;
using System.Reactive.Subjects;
using System.Threading;
using Acorisoft.FutureGL.Forest.Interfaces;

namespace Acorisoft.FutureGL.MigaStudio.Core
{
    public interface IAutoSaveService
    {
        IObservable<Unit> Observable { get; }
        IObservable<Unit> MinutesCounter { get; }
    }

    public class AutoSaveService : Disposable, IAutoSaveService
    {
        public const     int           MinutesToTick = 30;
        private readonly Subject<Unit> _subject;
        private readonly Subject<Unit> _perMinuteSubject;
        private readonly Timer         _timer;
        private          int           _secondCount;
        private          int           _triggerCount;
        
        public AutoSaveService()
        {
            _subject          = new Subject<Unit>();
            _perMinuteSubject = new Subject<Unit>();
            _triggerCount     = 100;
            _timer            = new Timer(DurationPushHandler, null, 0, 2000);
        }

        protected override void ReleaseManagedResources()
        {
            _perMinuteSubject.Dispose();
            _subject.Dispose();
            _timer.Dispose();
        }

        private void DurationPushHandler(object state)
        {
            _secondCount++;
            
            if (_secondCount >= _triggerCount)
            {
                _secondCount = 0;
                UIHelper.SuccessfulNotification(null, Language.GetText("text.OperationOfAutoSaveIsSuccessful"));
                #if DEBUG
                #else
                _subject.OnNext(Unit.Default);
                #endif
            }
            else if (_secondCount % 30 == 0)
            {
                _perMinuteSubject.OnNext(Unit.Default);
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
        public IObservable<Unit> MinutesCounter => _perMinuteSubject;
    }
}