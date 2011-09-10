module MathLib

open System

[<Measure>]
type rad

[<Measure>]
type deg

let pi = Math.PI * 1.<rad>

let sqrt = Math.Sqrt

let sin (x:float<rad>) = Math.Sin (x / 1.<rad>)

let cos (x:float<rad>) = Math.Cos (x / 1.<rad>)

let atan2 x y = Math.Atan2(x, y) * 1.<rad>

let degToRad (a:float<deg>) = (a / 180.0<deg>) * pi
