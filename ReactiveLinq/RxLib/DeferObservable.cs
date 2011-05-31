using System;

namespace RxLib
{
    public class DeferObservable<T> : IObservable<T>
    {
        private readonly Func<IObservable<T>> _func;

        public DeferObservable(Func<IObservable<T>> func)
        {
            _func = func;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return _func().Subscribe(observer);
        }

        public IObservable<T> GetInner()
        {
            return _func();
        }
    }
}