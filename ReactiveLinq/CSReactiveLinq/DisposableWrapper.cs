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

namespace CSReactiveLinq
{
    public class DisposableWrapper : IDisposable
    {
        private readonly Func<IDisposable>[] _xs;

        public DisposableWrapper(params Func<IDisposable>[] xs)
        {
            _xs = xs;
        }

        public void Dispose()
        {
            foreach (var x in _xs)
                x().Dispose();
        }
    }
}