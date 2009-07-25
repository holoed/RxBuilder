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
    public class ObserverWrapper<T, U> : IObserver<T>
    {
        private readonly Func<T, U> _selector;
        private readonly IObserver<U> _observer;

        public ObserverWrapper(Func<T, U> selector, IObserver<U> observer)
        {
            _selector = selector;
            _observer = observer;
        }

        public void OnNext(T value)
        {
            _observer.OnNext(_selector(value));
        }
    }
}