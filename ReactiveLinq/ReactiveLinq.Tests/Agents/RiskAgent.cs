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

namespace ReactiveLinq.Agents
{
    public class RiskAgent : IObservable<Risk>
    {        
        private readonly Random _rnd = new Random();
        private readonly List<IObserver<Risk>> _observers = new List<IObserver<Risk>>();

        public IDisposable Subscribe(IObserver<Risk> observer)
        {
            _observers.Add(observer);
            return new DisposeAction(() => _observers.Remove(observer));
        }

        public void Tick()
        {
            foreach (var observer in _observers)
                observer.OnNext(new Risk(_rnd.Next(0, 1000)));
        }

        public void Tick(string agg, string id)
        {
            foreach (var observer in _observers)
                observer.OnNext(new Risk(_rnd.Next(0, 1000)) { Aggregation = agg, Id = id });
        }
    }
}