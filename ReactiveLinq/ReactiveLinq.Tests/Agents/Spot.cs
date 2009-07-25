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
    public class Spot
    {
        private readonly double _price;

        public Spot(double price)
        {
            _price = price;
        }

        public override string ToString()
        {
            return "Spot: " + _price;
        }
    }
}