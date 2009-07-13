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

namespace Sample
{
    public class RiskAgent : IObservable<Risk>
    {        
        private readonly Random _rnd = new Random();
        private IObserver<Risk> _observer;

        public IDisposable Subscribe(IObserver<Risk> observer)
        {
            _observer = observer;
            return null;
        }

        public void Tick()
        {
            if (_observer != null)
                _observer.OnNext(new Risk(_rnd.Next(0, 1000)));
        }
    }
}