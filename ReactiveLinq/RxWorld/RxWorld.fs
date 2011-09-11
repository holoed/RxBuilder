module RxWorld

open System
open System.Windows
open RxLib
open MathLib
open Geometry2DLib
open Segment
open Vector

type Size = double * double

type Robot = { size : Size; pos : Segment; direction: float<deg>; speed: double }

type World = { size : unit -> Size; robot : Robot; obstacles : Segment list }

let obstacles = [Segment.create (Vector.create 100. 500.) (Vector.create 200. 600.);
                 Segment.create (Vector.create 600. 500.) (Vector.create 500. 600.)
                 Segment.create (Vector.create 500. 100.) (Vector.create 600. 200.)
                 Segment.create (Vector.create 200. 100.) (Vector.create 100. 200.)]

let initialState f = { size = f; robot = { size = (10.0, 10.0); pos = Segment.create (Vector.create 150.0 300.0) (Vector.create 150.0 300.0); direction = 90.<deg> ; speed = 20. }; obstacles = obstacles }


let livingRobot { size = f; robot = r } =
    let (w, h) = f() 
    let (rw, rh) = r.size
    let angle = degToRad r.direction
    let pos = Segment.p1 r.pos
    let vx = Vector.x pos
    let vy = Vector.y pos

    let distanceToObstacles = List.map (fun obstacle -> (obstacle, Segment.distanceBetweenSegmentAndPoint obstacle pos)) obstacles

    let (newDirection, newSpeed) =         
                match (List.tryFind (fun (_, d) -> d < 50.) distanceToObstacles) with
                | Some (obstacle, _) ->
                        let (v1, v2) = Segment.normals obstacle
                        let bounceVel = Vector.bounce 1.0 (Vector.fromPolar r.speed (degToRad r.direction)) v2
                        let newAngle = Vector.angle bounceVel            
                        let newSpeed = Vector.magnitude bounceVel
                        (newAngle, newSpeed)
                | None ->
                    (degToRad r.direction, r.speed)
                        
    { r with pos = Segment.create (Vector.create (vx + r.speed * (cos newDirection)) (vy + r.speed * (sin newDirection)))
                                  (Vector.create (vx + (r.speed + 50.) * (cos newDirection)) (vy + (r.speed + 50.) * (sin newDirection)))
             speed = newSpeed
             direction = radToDeg newDirection }
                                    
// live : World -> World
let livingWorld w = { w with robot = livingRobot w }

let Run (g:Func<Size>, f:Func<_,'b>) = timer (TimeSpan.FromSeconds 1.0) (TimeSpan.FromSeconds (1./30.)) 
                                          |> Observable.scan (fun x _ -> livingWorld x) (initialState (fun () -> g.Invoke()))
                                          |> Observable.map (fun x -> f.Invoke(x))


