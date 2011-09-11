namespace Geometry2D

open System
open Math

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