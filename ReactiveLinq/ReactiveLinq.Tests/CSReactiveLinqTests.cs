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

using System.Collections.Generic;
using NUnit.Framework;
using CSReactiveLinq;
using ReactiveLinq.Agents;

namespace ReactiveLinq
{
    [TestFixture]
    public class CSReactiveLinqTests : ReactiveLinqTests
    {
        [Test]
        public override void Select()
        {
            var q = from risk in risks
                    select risk;

            var riskList = new List<Risk>();

            q.Subscribe(riskList.Add);

            risks.Tick();
            risks.Tick();
            Assert.AreEqual(2, riskList.Count);
        }

        [Test]
        public override void SelectMany()
        {
            var q = from risk in risks
                    from spot in spots
                    select new { risk, spot };

            var list = newListOfType(q);

            q.Subscribe(list.Add);

            risks.Tick();
            spots.Tick();
            risks.Tick();
            spots.Tick();
            Assert.AreEqual(3, list.Count);
        }

        [Test]
        public override void GroupBy()
        {
            // C# version not implemented
        }
    }
}
