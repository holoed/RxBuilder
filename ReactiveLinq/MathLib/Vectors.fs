namespace Geometry2D

open System

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