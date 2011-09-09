module RxWorld

open System
open System.Windows
open RxLib
open MathLib



type Size = double * double

type Position = double * double

type Angle = double

type Speed = double

type Robot = { size : Size; pos : Position; direction: Angle; speed: Speed }

type World = { size : unit -> Size; robot : Robot }

let initialState f = { size = f; robot = { size = (10.0, 10.0); pos = (5.0, 5.0); direction = Math.PI * 1. / 10.; speed = 10. } }

let livingRobot { size = f; robot = r } =
    let (w, h) = f() 
    let (rw, rh) = r.size
    let (x, y) = r.pos
    let angle = r.direction
                
    { r with pos = (x + r.speed * (Math.Sin angle), 
                    y + r.speed * (Math.Cos angle))
             direction = angle }
                                    
// live : World -> World
let livingWorld w = { w with robot = livingRobot w }

let Run (g:Func<Size>, f:Func<_,'b>) = timer (TimeSpan.FromSeconds 1.0) (TimeSpan.FromSeconds (1./60.)) 
                                          |> Observable.scan (fun x _ -> livingWorld x) (initialState (fun () -> g.Invoke()))
                                          |> Observable.map (fun x -> f.Invoke(x))


