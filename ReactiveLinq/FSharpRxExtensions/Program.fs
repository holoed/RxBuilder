open System
open System.Linq
open System.Reactive.Linq
open System.Reactive.Concurrency
open RxLib

//*************************************************************************
// Fundamentals
//*************************************************************************

type 'a Observable = Observable of (('a -> unit) -> unit)

let subscribe obv (Observable f) = f obv

let unit x = Observable (fun k -> k x)

let delay f = Observable (fun k -> f () |> subscribe k)

let append xs ys = Observable (fun obv -> xs |> subscribe obv
                                          ys |> subscribe obv)

let rec naturals n = append (unit n) (delay (fun () -> naturals (n + 1)))

//do (naturals 5) |> subscribe (printfn "%A")


//*************************************************************************
// Rx Builder
//*************************************************************************

type rxBuilder() =    
   member this.Delay (f : unit -> 'a IObservable) = 
               { new IObservable<_> with
                    member this.Subscribe obv = (f()).Subscribe obv }
   member this.Combine (xs:'a IObservable, ys: 'a IObservable) =
               { new IObservable<_> with
                    member this.Subscribe obv = xs.Subscribe obv ; 
                                                ys.Subscribe obv }
   member this.Yield x = Observable.Return x
   member this.YieldFrom xs = xs

let rx = rxBuilder()

let rec f x = rx { yield x 
                   yield! f (x + 1) }

do f 5 |> Observable.subscribe (fun x -> Console.WriteLine x) |> ignore

do System.Console.ReadLine() |> ignore