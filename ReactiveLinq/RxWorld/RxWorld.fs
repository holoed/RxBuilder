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

type World = { size : unit -> Size; robot : Robot }

let initialState f = { size = f; robot = { size = (10.0, 10.0); pos = Segment.create (Vector.create 150.0 5.0) (Vector.create 150.0 5.0); direction = 90.<deg> ; speed = 10. } }

let obstacles = [Segment.create (Vector.create 100. 100.) (Vector.create 200. 200.);
                 Segment.create (Vector.create 600. 100.) (Vector.create 500. 200.)]

let livingRobot { size = f; robot = r } =
    let (w, h) = f() 
    let (rw, rh) = r.size
    let angle = degToRad r.direction
    let pos = Segment.p1 r.pos
    let vx = Vector.x pos
    let vy = Vector.y pos

    let distanceToObstacles = List.map (fun obstacle -> (obstacle, Segment.distanceBetweenSegmentAndPoint obstacle pos)) obstacles

    let (newDirection, newSpeed) = 
        let hitObstacle = List.tryFind (fun (_, d) -> d < 50.) distanceToObstacles
        if (hitObstacle.IsSome) then 
            let (Some (obstacle, _)) = hitObstacle
            let (v1, v2) = Segment.normals obstacle
            let bounceVel = Vector.bounce (Vector.fromPolar r.speed (degToRad r.direction)) v2
            let newAngle = Vector.angle bounceVel            
            (newAngle, r.speed)
        else 
            (degToRad r.direction, r.speed)
                        
    { r with pos = Segment.create (Vector.create (vx + r.speed * (cos newDirection)) (vy + r.speed * (sin newDirection)))
                                  (Vector.create (vx + (r.speed + 50.) * (cos newDirection)) (vy + (r.speed + 50.) * (sin newDirection)))
             speed = newSpeed
             direction = radToDeg newDirection }
                                    
// live : World -> World
let livingWorld w = { w with robot = livingRobot w }

let Run (g:Func<Size>, f:Func<_,'b>) = timer (TimeSpan.FromSeconds 1.0) (TimeSpan.FromSeconds (1./10.)) 
                                          |> Observable.scan (fun x _ -> livingWorld x) (initialState (fun () -> g.Invoke()))
                                          |> Observable.map (fun x -> f.Invoke(x))


