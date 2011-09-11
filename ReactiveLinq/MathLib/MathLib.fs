module MathLib

open System

[<Measure>]
type rad

[<Measure>]
type deg

//  pi : float<deg>
let pi = Math.PI * 1.<rad>

//  sqrt : float -> float
let sqrt = Math.Sqrt

//  sin : float<rad> -> float
let sin x = Math.Sin (x / 1.<rad>)

//  cos : float<rad> -> float
let cos x = Math.Cos (x / 1.<rad>)

//  atan2 : float -> float -> float<rad>
let atan2 x y = Math.Atan2(x, y) * 1.<rad>

//  degToRad : float<deg> -> float<rad>
let degToRad (a:float<deg>) = (a * pi) / 180.<deg>

//  radToDeg : float<rad> -> float<deg>
let radToDeg (a:float<rad>) = (a * 180.0<deg>) / pi
