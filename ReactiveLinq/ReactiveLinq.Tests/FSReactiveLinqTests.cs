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
using FSReactiveLinq;
using ReactiveLinq.Agents;
using NUnit.Framework;

namespace ReactiveLinq
{
    [TestFixture]
    public class FSReactiveLinqTests : ReactiveLinqTests
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
            var q = from risk in risks
                    group risk by risk.Aggregation into g
                    select new { g.Key, Value = g };

            var byPosList = new List<Risk>();
            var byUndList = new List<Risk>();

            int called = 0;
            int calledPos = 0;
            int calledUnd = 0;

            Func<string, Action<Risk>> f =
                key =>
                {
                    called++;
                    if (key == "byPos") { calledPos++; return byPosList.Add; }
                    if (key == "byUnd") { calledUnd++; return byUndList.Add; }
                    return x => { };
                };

            q.Subscribe(item => item.Value.Subscribe(f(item.Key)  ));

            risks.Tick("byUnd", "VOD.L");
            risks.Tick("byUnd", "MSFT");
            risks.Tick("byPos", "00001");
            risks.Tick("byPos", "00002");
            risks.Tick("byUnd", "ORCL");
            risks.Tick("byPos", "00001");

            Assert.AreEqual(3, byUndList.Count);
            Assert.AreEqual(3, byPosList.Count);

            Assert.AreEqual("VOD.L", byUndList[0].Id);
            Assert.AreEqual("MSFT", byUndList[1].Id);
            Assert.AreEqual("ORCL", byUndList[2].Id);

            Assert.AreEqual("00001", byPosList[0].Id);
            Assert.AreEqual("00002", byPosList[1].Id);
            Assert.AreEqual("00001", byPosList[2].Id);

            Assert.AreEqual(2, called);
            Assert.AreEqual(1, calledUnd);
            Assert.AreEqual(1, calledPos);
        }

        [Test]
        public override void Dispose()
        {
            var q = from risk in risks
                    select risk;

            var xs = new List<Risk>();

            Assert.AreEqual(0, xs.Count);
            using (q.Subscribe(xs.Add))
            {
                risks.Tick();
                Assert.AreEqual(1, xs.Count);
            }
            risks.Tick();
            Assert.AreEqual(1, xs.Count);
        }

        [Test]
        public override void DisposeSelectMany()
        {
            var q = from risk in risks
                    from spot in spots
                    select new {risk, spot};

            var xs = newListOfType(q);

            Assert.AreEqual(0, xs.Count);
            using (q.Subscribe(xs.Add))
            {
                risks.Tick();
                spots.Tick();
                Assert.AreEqual(1, xs.Count);
            }
            risks.Tick();
            spots.Tick();
            Assert.AreEqual(1, xs.Count);
        }
    }
}
