open System
open System.Linq
open System.Reactive.Linq
open System.Reactive.Concurrency
open RxLib


type rxBuilder() =    
   member this.Delay f = ObservableExt.Defer f
   member this.Combine (xs:'a IObservable, ys: 'a IObservable) =
               ObservableExt.Combine (xs, ys)
   member this.Yield x = Observable.Return x
   member this.YieldFrom xs = xs

let rx = rxBuilder()

let rec f x = rx { yield x 
                   yield! f (x + 1) }

do f 5 |> fun xs -> Observable.ObserveOn(xs, Scheduler.CurrentThread) |> Observable.subscribe (fun x -> Console.WriteLine x) |> ignore

do System.Console.ReadLine() |> ignore