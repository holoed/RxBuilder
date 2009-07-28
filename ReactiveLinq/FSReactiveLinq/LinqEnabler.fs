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
//
//type IObservableGrouping<'a,'b> =
//    inherit IObservable<'b>
//    abstract member Key: 'a with get
    
let CreateObserver f (next: IObserver<'b>) = { new IObserver<'a> with 
                                                               member o.OnNext(x) = 
                                                                   next.OnNext(f x) }
                                                                   
let Map(observable: IObservable<'a>, f) 
                    = { new IObservable<'b> with 
                            member o.Subscribe(observer) =
                                observable.Subscribe(CreateObserver f observer) }
                                                                     
let Bind(observable: IObservable<'a>, selector : 'a -> IObservable<'b>, projection) =
 
                    let project (observer:IObserver<'c>) x = CreateObserver (projection x) observer
                       
                    //TODO: Remove mutable Reference Cell   
                    let disposableB = ref null
                                                              
                    let Subscribe observer = { new IObserver<'a> with 
                                                   member o.OnNext x = 
                                                         disposableB := ((selector x).Subscribe(project observer x)) } 
                    
                    {  new IObservable<'c> with 
                          member o.Subscribe(observer) =
                              let disposableA = observable.Subscribe(Subscribe observer)
                              { new IDisposable with
                                    member o.Dispose() =
                                          disposableA.Dispose()
                                          disposableB.Value.Dispose() } }
                                     

[<Extension>]
let Select(observable : IObservable<'a>, selector : Func<'a,'b>) = 
    Map(observable, fun x -> selector.Invoke(x))
     
[<Extension>]
let SelectMany(observable : IObservable<'a>, selector : Func<'a, IObservable<'b>>, projection : Func<'a, 'b, 'c>) = 
    Bind(observable, (fun x -> selector.Invoke(x)), (fun x -> fun y -> projection.Invoke(x, y)))
    
[<Extension>]
let GroupBy(source : IObservable<'a>, keySelector : Func<'a, 'b>) =
    let dict = new System.Collections.Generic.Dictionary<'b, bool * IObservableGrouping<'b, 'a> * ('a -> unit)>()

    { new IObservable<IObservableGrouping<'b, 'a>>
        with member o.Subscribe(observer) = 
                 source.Subscribe({ new IObserver<'a> with 
                                        member o.OnNext x =
                                               let y = keySelector.Invoke x
                                               let getValue key value =
                                                    if (not (dict.ContainsKey key)) then
                                                         dict.Add (key, (false, { new IObservableGrouping<'b, 'a> with 
                                                                                member o.Key = y                                                        
                                                                                member o.Subscribe(observer') = 
                                                                                        observer'.OnNext value
                                                                                        let (_, obv, _) = dict.[y]
                                                                                        dict.[y] <- (true, obv, fun x' -> observer'.OnNext x')
                                                                                        null }, fun x' -> ()))
                                                    dict.[key]
                                               let (subs, obv, f) = getValue y x
                                               if (not subs) then observer.OnNext(obv)
                                               f(x) }) }
                               
                                    
[<Extension>]
let Subscribe(observable : IObservable<'a>)  (action:Action<'a>) =
        let observer = { new IObserver<'a>
                            with member o.OnNext x = action.Invoke x }
        observable.Subscribe(observer)
                