using System;
using System.Reactive.Disposables;
using System.Reactive.Concurrency;
using System.Threading;

namespace RxLib
{
    public class CombineObservable<T> : IObservable<T>
    {
        private IObservable<T> _xs;
        private IObservable<T> _ys;

        public CombineObservable(IObservable<T> xs, IObservable<T> ys)
        {
            _xs = xs;
            _ys = ys;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            var mutableDisposable = new MutableDisposable(AssignmentBehavior.ReplaceAndDisposePrevious);
            var handle = new ManualResetEvent(false);
            Scheduler.ThreadPool.Schedule(() => ScheduleNextObservable(handle, mutableDisposable, observer));
            mutableDisposable.Disposable = _xs.Subscribe(observer.OnNext, observer.OnError, () => handle.Set());
            return mutableDisposable;
        }

        private void ScheduleNextObservable(ManualResetEvent handle, MutableDisposable mutableDisposable, IObserver<T> observer)
        {
            handle.WaitOne();
            handle.Close();
            var deferObservable = (DeferObservable<T>)_ys;
            var innerObservable = deferObservable.GetInner();
            while (innerObservable is DeferObservable<T>)
                innerObservable = ((DeferObservable<T>)innerObservable).GetInner();
            var combineObservable = ((CombineObservable<T>)innerObservable);
            _xs = combineObservable.GetLeftObservable();
            _ys = combineObservable.GetRightObservable();
            mutableDisposable.Disposable = Subscribe(observer);
        }

        private IObservable<T> GetLeftObservable()
        {
            return _xs;
        }

        private IObservable<T> GetRightObservable()
        {
            return _ys;
        }
    }
}