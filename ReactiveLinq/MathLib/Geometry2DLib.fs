module Geometry2DLib

open System
open MathLib

type Vector = 
        private { x:float; y:float } 
        static member (+) (v1, v2) = { x = v1.x + v2.x; y = v1.y + v2.y }
        static member (-) (v1, v2) = { x = v1.x - v2.x; y = v1.y - v2.y }
        static member (*) (k, v) = { x = v.x * k; y = v.y * k }
        static member (*) (v, k) = { x = v.x * k; y = v.y * k }

let vector x y = { x = x; y = y }

let magnitude v = sqrt (v.x * v.x + v.y * v.y)

let angle v = atan2 v.x v.y

let normalize v = let size = magnitude v
                  { x = v.x / size; y = v.y / size }

let cross v1 v2 = (v1.x * v2.y) - (v1.y * v2.x)

let dot v1 v2 = (v1.x * v2.x) + (v1.y * v2.y);

let fromPolar rho theta = { x = rho * cos theta
                            y = rho * sin theta }

let getX v = v.x

let getY v = v.y
                
type Segment = private { p1: Vector; p2 : Vector }

let segment p1 p2 = { p1 = p1; p2 = p2 }

let getP1 s = s.p1

let getP2 s = s.p2

let normals s = let dv = s.p2 - s.p1
                ({ x = -dv.y; y = dv.x}, { x = dv.y; y = -dv.x })

let distance v1 v2 = sqrt((v2.x - v1.x) * (v2.x - v1.x)) + ((v2.y - v1.y) * (v2.y - v1.y));

let midpoint s = { x = (s.p1.x + s.p2.x) / 2.0
                   y = (s.p1.y + s.p2.y) / 2.0 }

let bounce v n = let n = normalize n
                 let d = dot n v
                 - 2.0 * (d * n) + v

let getP1X = getP1 >> getX

let getP1Y = getP1 >> getY

let getP2X = getP2 >> getX

let getP2Y = getP2 >> getY

let distanceBetweenSegmentAndPoint s p =
    let v = { x = s.p2.x - s.p1.x
              y = s.p2.y - s.p1.y }
    let w = { x = p.x - s.p1.x
              y = p.y - s.p1.y }
    let c1 = dot w v
    let c2 = dot v v
    if (c1 <= 0. ) then
        distance p s.p1
    elif (c2 <= c1) then
        distance p s.p2
    else
        let b = c1 / c2
        let pb = { x = s.p1.x + b * v.x 
                   y = s.p1.y + b * v.y }
        distance p pb