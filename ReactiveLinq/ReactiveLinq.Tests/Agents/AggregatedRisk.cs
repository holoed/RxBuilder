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
using System.Linq;
using ReactiveLinq.Agents;

namespace ReactiveLinq.Agents
{
    public class AggregatedRisk : Risk
    {
        private readonly Dictionary<string, Risk> _items = new Dictionary<string, Risk>();

        public static AggregatedRisk operator+(AggregatedRisk left, Risk right)
        {
            left.Add(right);
            return left;
        }

        private void Add(Risk risk)
        {
            _items[risk.Id] = risk;
            Delta = _items
                .Select(item => item.Value.Delta)
                .Aggregate((x, y) => x + y);
            Underlying = risk.Underlying;
        } 
    }
}