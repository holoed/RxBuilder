namespace Geometry2D

open System

[<Measure>]
type rad

[<Measure>]
type deg

[<AutoOpen>]
module Math =

    //  pi : float<deg>
    let pi = Math.PI * 1.<rad>

    //  sin : float<rad> -> float
    let sin x = sin (x / 1.<rad>)

    //  cos : float<rad> -> float
    let cos x = cos (x / 1.<rad>)

    //  atan2 : float -> float -> float<rad>
    let atan2 x y = (atan2 x y) * 1.<rad>

    //  degToRad : float<deg> -> float<rad>
    let degToRad (a:float<deg>) = (a * pi) / 180.<deg>

    //  radToDeg : float<rad> -> float<deg>
    let radToDeg (a:float<rad>) = (a * 180.0<deg>) / pi
