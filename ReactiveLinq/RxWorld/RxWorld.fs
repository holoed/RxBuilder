module RxWorld

open System
open System.Windows
open FSharpReactiveExtensions
open Geometry2D
open Segments
open Vectors

type Size = double * double

type Robot = { size : Size; pos : Segment; direction: float<deg>; speed: double }

type World = { size : unit -> Size; robot : Robot; obstacles : Segment list }

let obstacles = [Segments.create (Vectors.create 100. 500.) (Vectors.create 200. 600.);
                 Segments.create (Vectors.create 600. 500.) (Vectors.create 500. 600.)
                 Segments.create (Vectors.create 500. 100.) (Vectors.create 600. 200.)
                 Segments.create (Vectors.create 200. 100.) (Vectors.create 100. 200.)]

let initialState f = { size = f; robot = { size = (10.0, 10.0); pos = Segments.create (Vectors.create 150.0 300.0) (Vectors.create 150.0 300.0); direction = 90.<deg> ; speed = 20. }; obstacles = obstacles }


let livingRobot { size = f; robot = r } =
    let (w, h) = f() 
    let (rw, rh) = r.size
    let angle = degToRad r.direction
    let pos = Segments.p1 r.pos
    let vx = Vectors.x pos
    let vy = Vectors.y pos

    let distanceToObstacles = List.map (fun obstacle -> (obstacle, Segments.distanceBetweenSegmentAndPoint obstacle pos)) obstacles

    let (newDirection, newSpeed) =         
                match (List.tryFind (fun (_, d) -> d < 50.) distanceToObstacles) with
                | Some (obstacle, _) ->
                        let (v1, v2) = Segments.normals obstacle
                        let bounceVel = Vectors.bounce 1.0 (Vectors.fromPolar r.speed (degToRad r.direction)) v2
                        let newAngle = Vectors.angle bounceVel            
                        let newSpeed = Vectors.magnitude bounceVel
                        (newAngle, newSpeed)
                | None ->
                    (degToRad r.direction, r.speed)
                        
    { r with pos = Segments.create (Vectors.create (vx + r.speed * (cos newDirection)) (vy + r.speed * (sin newDirection)))
                                   (Vectors.create (vx + (r.speed + 50.) * (cos newDirection)) (vy + (r.speed + 50.) * (sin newDirection)))
             speed = newSpeed
             direction = radToDeg newDirection }
                                    
// live : World -> World
let livingWorld w = { w with robot = livingRobot w }

let Run (g:Func<Size>, f:Func<_,'b>) = Observable.timer (TimeSpan.FromSeconds 1.0) (TimeSpan.FromSeconds (1./30.)) 
                                          |> Observable.scan (fun x _ -> livingWorld x) (initialState (fun () -> g.Invoke()))
                                          |> Observable.map (fun x -> f.Invoke(x))


