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
    public static class Observer
    {
        public static IDisposable Subscribe<T>(this IObservable<T> observable, Action<T> action)
        {
            return observable.Subscribe(new Observer<T>(action));
        }
    }

    public class Observer<T> : IObserver<T>
    {
        private readonly Action<T> _action;

        public Observer(Action<T> action)
        {
            _action = action;
        }

        public void OnNext(T value)
        {
            if (_action != null)
                _action(value);
        }
    }
}