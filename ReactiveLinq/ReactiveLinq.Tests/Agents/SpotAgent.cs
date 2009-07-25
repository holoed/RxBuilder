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
using System.Collections.Generic;
using CSReactiveLinq;

namespace ReactiveLinq.Agents
{
    public class SpotAgent : IObservable<Spot>
    {
        private readonly Random _rnd = new Random();
        private readonly List<IObserver<Spot>> _observers = new List<IObserver<Spot>>();

        public IDisposable Subscribe(IObserver<Spot> observer)
        {
            _observers.Add(observer);
            return new DisposeAction(() => _observers.Remove(observer));
        }

        public void Tick()
        {
            var spot = new Spot(_rnd.Next(0, 500));
            foreach (var observer in _observers)
                observer.OnNext(spot);
        }
    }
}