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

namespace ReactiveLinq
{
    public static class Observer
    {
        public static IObserver<T> Subscribe<T>(IObservable<T> observable, Action<T> action)
        {
            var observer = new Observer<T>(action);
            observable.Subscribe(observer);
            return observer;
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