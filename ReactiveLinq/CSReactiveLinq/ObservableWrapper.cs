#region License

/* ****************************************************************************
 * Copyright (c) Edmondo Pentangelo. 
 *
 * This source code is subject to terms and conditions of the Microsoft Public License. 
 * A copy of the license can be found in the License.html file at the root of this distribution. 
 * By using this source code in any fashion, you are agreeing to be bound by the terms of the 
 * Microsoft Public License.
 *
 * You must not remove this notice, or any other, from this software.
 * ***************************************************************************/

#endregion

using System;
using ReactiveLinq;

namespace CSReactiveLinq
{
    public class ObservableWrapper<T, U> : IObservable<U>
    {
        private readonly IObservable<T> _observable;
        private readonly Func<T, U> _selector;

        public ObservableWrapper(IObservable<T> observable, Func<T, U> selector)
        {
            _observable = observable;
            _selector = selector;
        }

        public IDisposable Subscribe(IObserver<U> o)
        {
            return _observable.Subscribe(new ObserverWrapper<T, U>(_selector, o));
        }
    }

    public class ObservableWrapper<T, U, V> : IObservable<V>
    {
        private readonly Func<T, U, V> _projector;
        private readonly Func<T, IObservable<U>> _selector;
        private readonly IObservable<T> _p;

        public ObservableWrapper(IObservable<T> p, Func<T, IObservable<U>> selector, Func<T, U, V> projector)
        {
            _p = p;
            _selector = selector;
            _projector = projector;
        }

        public IDisposable Subscribe(IObserver<V> observerV)
        {
            IDisposable disposableY = null;

            var observerT = new Observer<T>(
                t => { disposableY = _selector(t).Subscribe(new Observer<U>(u => observerV.OnNext(_projector(t, u)))); });
            var disposableX = _p.Subscribe(observerT);

            return new DisposableWrapper(() => disposableX, () => disposableY);
        }
    }
}