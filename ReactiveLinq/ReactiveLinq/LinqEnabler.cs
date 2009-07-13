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
    public static class LinqEnabler
    {
        public static IObservable<U> Select<T, U>(this IObservable<T> p, Func<T, U> selector)
        {
            return new ObservableWrapper<T, U>(p, selector);
        }

        public static IObservable<V> SelectMany<T, U, V>(this IObservable<T> p, Func<T, IObservable<U>> selector, Func<T, U, V> projector)
        {
            return new ObservableWrapper<T, U, V>(p, selector, projector);
        }
    }
}