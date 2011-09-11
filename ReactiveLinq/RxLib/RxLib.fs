namespace FSharpReactiveExtensions

open System
open System.Linq
open System.Reactive.Linq

type rxBuilder() =    
    member this.Bind ((xs:'a IObservable), (f:'a -> 'b IObservable)) =
        Observable.SelectMany (xs, f)
    member this.Delay f = Observable.Defer f
    member this.Return x = Observable.Return x
    member this.ReturnFrom xs = xs
    member this.Combine (xs:'a IObservable, ys: 'a IObservable) =
        Observable.Concat (xs, ys)
    member this.For (xs : 'a seq, f: 'a -> 'b IObservable) =
        Observable.For(xs, new Func<_, IObservable<_>>(f)) 
    member this.TryFinally (xs: 'a IObservable, f : unit -> unit) =
        Observable.Finally(xs, new Action(f))
    member this.TryWith (xs: 'a IObservable, f: exn -> 'a IObservable) =
        Observable.Catch (xs, new Func<exn, 'a IObservable>(f))
    member this.While (f, xs: 'a IObservable) =
        Observable.While (new Func<bool>(f), xs)
    member this.Yield x = Observable.Return x
    member this.YieldFrom xs = xs
    member this.Zero () = Observable.Empty()
              
module Observable = 

    let rx = rxBuilder()

    // Rx combinators

    // unit :: 'a -> IObservable<'a>
    let unit = Observable.Return

    //  repeat : IObservable<'a> -> IObservable<'a>
    let repeat (xs:IObservable<_>) = xs.Repeat()

    //  timer : TimeSpan -> TimeSpan -> IObservable<int64>
    let timer (dueTime:TimeSpan) (period:TimeSpan) = Observable.Timer (dueTime, period)

    //  startWith : IObservable<'a> -> IObservable<'a>
    let startWith = Observable.StartWith

    // timeStamp :: IObservable<'a> -> IObservable<Timestamped<'a>>
    let timeStamp = Observable.Timestamp

    // zip :: IObservable<'a> -> IObservable<'b> -> ('a -> 'b -> 'c) -> 'IObservable<'c>
    let zip (xs : IObservable<_>) (ys : IObservable<_>) f = Observable.Zip(xs, ys, new Func<_,_,_>(f))

    // skip :: IObservable<'a> -> int -> IObservable<'a>
    let skip xs i = Observable.Skip (xs, i)

    let liftM f xs = rx { let! x = xs
                          return f x }

    let liftM2 f xs ys = rx { let! x = xs
                              let! y = ys
                              return f x y }


