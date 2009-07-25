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
using System.Linq;
using System.Text;
using NUnit.Framework;
using ReactiveLinq.Agents;

namespace ReactiveLinq
{
    public abstract class ReactiveLinqTests
    {
        protected SpotAgent spots = new SpotAgent();
        protected RiskAgent risks = new RiskAgent();

        public abstract void Select();
        public abstract void SelectMany();
        public abstract void GroupBy();

        protected List<T> newListOfType<T>(IObservable<T> q)
        {
            return new List<T>();
        }

    }
}
