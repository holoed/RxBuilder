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

namespace ReactiveLinq.Agents
{
    public class Risk 
    {
        public string Aggregation;
        public string Id;
        private readonly double _delta;

        public Risk(double delta)
        {
            _delta = delta;
        }

        public override string ToString()
        {
            return string.Format("ID:{0}\tDelta{1}", Id, _delta);
        }
    }
}