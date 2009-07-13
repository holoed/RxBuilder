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
    class Program
    {
        static void Main(string[] args)
        {
            var spots = new SpotAgent();
            var risks = new RiskAgent();

            //var q = from spot in spots
            //        select spot;

            var q = from spot in spots
                    from risk in risks
                    select new { spot, risk };

            Observer.Subscribe(q, Console.WriteLine);

            risks.Tick();

            spots.Tick();

            risks.Tick();
            risks.Tick();
            risks.Tick();

            spots.Tick();

            risks.Tick();
            risks.Tick();
            risks.Tick();
        }
    }
}
