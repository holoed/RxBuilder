using System;

namespace RxLib
{
    public static class ObservableExt
    {
        public static IObservable<T> Defer<T>(Func<IObservable<T>> func)
        {
            return new DeferObservable<T>(func);
        }

        public static IObservable<T> Combine<T>(this IObservable<T> xs,
                                                IObservable<T> ys)
        {
            return new CombineObservable<T>(xs, ys);
        }
    }
}