namespace Geometry2DLib

open System
open MathLib

type Vector  = 
        private { x: float; y: float } 
        static member (+) (v1, v2) = { x = v1.x + v2.x; y = v1.y + v2.y }
        static member (-) (v1, v2) = { x = v1.x - v2.x; y = v1.y - v2.y }
        static member (*) (k, v) = { x = v.x * k; y = v.y * k }
        static member (*) (v, k) = { x = v.x * k; y = v.y * k }

module Vectors =

    //  create : float -> float -> Vector
    let create x y = { x = x; y = y }

    //  map : (float -> float) -> Vector -> Vector
    let map f v = { x = f v.x ; y = f v.y }

    //  fold : ('a -> float -> 'a) -> 'a -> Vector -> 'a
    let fold f acc v = List.fold f acc [v.x; v.y] 

    //  magnitude : Vector -> float
    let magnitude v = sqrt (v.x * v.x + v.y * v.y)

    //  angle : Vector -> float<rad>
    let angle v = atan2 v.y v.x

    //  normalize : Vector -> Vector
    let normalize v = let size = magnitude v
                      map (fun x -> x / size) v

    //  cross : Vector -> Vector -> float
    let cross v1 v2 = (v1.x * v2.y) - (v1.y * v2.x)

    //  dot : Vector -> Vector -> float
    let dot v1 v2 = (v1.x * v2.x) + (v1.y * v2.y);

    //  fromPolar : float -> float<rad> -> Vector
    let fromPolar rho theta = { x = rho * cos theta
                                y = rho * sin theta }

    //  x : Vector -> float
    let x v = v.x

    //  y : Vector -> float
    let y v = v.y

    //  distance : Vector -> Vector -> float
    let distance v1 v2 = sqrt((x v2 - x v1) * (x v2 - x v1)) + ((y v2 - y v1) * (y v2 - y v1));

    //  bounce : float -> Vector -> Vector -> Vector
    let bounce b v n = let n = normalize n
                       let d = dot n v
                       b * (- 2.0 * (d * n) + v)

type Segment = private { p1: Vector; p2 : Vector }

module Segments =

    open Vectors
               
    //  create : Vector -> Vector -> Segment                    
    let create p1 p2 = { p1 = p1; p2 = p2 }

    //  p1 : Segment -> Vector
    let p1 s = s.p1

    //  p2 : Segment -> Vector
    let p2 s = s.p2

    //  normals : Segment -> Vector * Vector
    let normals s = let dv = s.p2 - s.p1
                    (Vectors.create (- y dv) (x dv), Vectors.create (y dv) (- x dv))

    //  midpoint : Segment -> Vector    
    let midpoint s = Vectors.create ((x s.p1 + x s.p2) / 2.0) ((y s.p1 + y s.p2) / 2.0)

    //  p1x : Segment -> float
    let p1x = p1 >> x

    //  p1y : Segment -> float
    let p1y = p1 >> y

    //  p2x : Segment -> float
    let p2x = p2 >> x

    //  p2y : Segment -> float
    let p2y = p2 >> y

    //  distanceBetweenSegmentAndPoint : Segment -> Vector -> float
    let distanceBetweenSegmentAndPoint s p =
        let v = Vectors.create (x s.p2 - x s.p1) (y s.p2 - y s.p1)
        let w = Vectors.create (x p - x s.p1) (y p - y s.p1)
        let c1 = dot w v
        let c2 = dot v v
        if (c1 <= 0. ) then
            distance p s.p1
        elif (c2 <= c1) then
            distance p s.p2
        else
            let b = c1 / c2
            let pb = Vectors.create (x s.p1 + b * x v) (y s.p1 + b * y v)
            distance p pb