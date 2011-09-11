module Geometry2DLib

open System
open MathLib

module Vector =

    type Vector = 
            private { x:float; y:float } 
            static member (+) (v1, v2) = { x = v1.x + v2.x; y = v1.y + v2.y }
            static member (-) (v1, v2) = { x = v1.x - v2.x; y = v1.y - v2.y }
            static member (*) (k, v) = { x = v.x * k; y = v.y * k }
            static member (*) (v, k) = { x = v.x * k; y = v.y * k }

    let create x y = { x = x; y = y }

    let magnitude v = sqrt (v.x * v.x + v.y * v.y)

    let angle v = atan2 v.y v.x

    let normalize v = let size = magnitude v
                      { x = v.x / size; y = v.y / size }

    let cross v1 v2 = (v1.x * v2.y) - (v1.y * v2.x)

    let dot v1 v2 = (v1.x * v2.x) + (v1.y * v2.y);

    let fromPolar rho theta = { x = rho * cos theta
                                y = rho * sin theta }

    let x v = v.x

    let y v = v.y

    let bounce b v n = let n = normalize n
                       let d = dot n v
                       b * (- 2.0 * (d * n) + v)
module Segment =

    open Vector

    type Segment = private { p1: Vector; p2 : Vector }
                
    let create p1 p2 = { p1 = p1; p2 = p2 }

    let p1 s = s.p1

    let p2 s = s.p2

    let normals s = let dv = s.p2 - s.p1
                    (Vector.create (- y dv) (x dv), Vector.create (y dv) (- x dv))

    let distance v1 v2 = sqrt((x v2 - x v1) * (x v2 - x v1)) + ((y v2 - y v1) * (y v2 - y v1));

    let midpoint s = Vector.create ((x s.p1 + x s.p2) / 2.0) ((y s.p1 + y s.p2) / 2.0)

    let p1x = p1 >> x

    let p1y = p1 >> y

    let p2x = p2 >> x

    let p2y = p2 >> y

    let distanceBetweenSegmentAndPoint s p =
        let v = Vector.create (x s.p2 - x s.p1) (y s.p2 - y s.p1)
        let w = Vector.create (x p - x s.p1) (y p - y s.p1)
        let c1 = dot w v
        let c2 = dot v v
        if (c1 <= 0. ) then
            distance p s.p1
        elif (c2 <= c1) then
            distance p s.p2
        else
            let b = c1 / c2
            let pb = Vector.create (x s.p1 + b * x v) (y s.p1 + b * y v)
            distance p pb