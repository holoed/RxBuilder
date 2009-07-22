(* ****************************************************************************
 * Copyright (c) Edmondo Pentangelo. 
 *
 * This source code is subject to terms and conditions of the Microsoft Public License. 
 * A copy of the license can be found in the License.html file at the root of this distribution. 
 * By using this source code in any fashion, you are agreeing to be bound by the terms of the 
 * Microsoft Public License.
 *
 * You must not remove this notice, or any other, from this software.
 * ***************************************************************************)

#light

namespace FSReactiveLinq
[<System.Runtime.CompilerServices.Extension>]
module LinqEnabler

open System
open System.Runtime.CompilerServices
open ReactiveLinq

// IObservable interfaces

//type IObserver<'a> = 
//    abstract member OnNext : 'a -> unit
//
//type IObservable<'a> = 
//    abstract member Subscribe : IObserver<'a> -> IDisposable
    
// Bind and Return implentations compatible with Linq

let CreateObserver<'a, 'b> f (next: IObserver<'b>) = { new IObserver<'a>
                                                          with member o.OnNext(x) = 
                                                                   next.OnNext(f x) }
                                                                   
let Return<'a, 'b>(observable: IObservable<'a>, selector) 
                    = { new IObservable<'b>
                         with member o.Subscribe(observer) =
                                observable.Subscribe(CreateObserver<'a, 'b> selector observer) }
                                                                     
let Bind<'a, 'b, 'c>(observable: IObservable<'a>, selector : 'a -> IObservable<'b>, projection) =
                    
                    let project (observer:IObserver<'c>) x = CreateObserver<'b, 'c> (projection x) observer
                                                                 
                    let Subscribe observer = { new IObserver<'a>
                                                 with member o.OnNext x = 
                                                         //TODO: Add support for IDisposable
                                                         ignore ((selector x).Subscribe(project observer x)) } 
                    
                    {  new IObservable<'c>
                       with member o.Subscribe(observer) =
                              observable.Subscribe(Subscribe observer) }
                                     

[<Extension>]
let Select(observable : IObservable<'a>, selector : Func<'a,'b>) = 
    Return<'a, 'b>(observable, fun x -> selector.Invoke(x))
     
[<Extension>]
let SelectMany(observable : IObservable<'a>, selector : Func<'a, IObservable<'b>>, projection : Func<'a, 'b, 'c>) = 
    Bind<'a, 'b, 'c>(observable, (fun x -> selector.Invoke(x)), (fun x -> fun y -> projection.Invoke(x, y)))
    

// Utility function to easy subscription
let Subscribe<'a> (observable : IObservable<'a>)  (action:Action<'a>) =
        let observer = { new IObserver<'a>
                            with member o.OnNext x = action.Invoke x }
        observable.Subscribe(observer)
                