module RobotBehaviours

open System
open System.Reactive.Linq
open FSharpReactiveExtensions

let pairB xs ys = Observable.zip xs ys (fun x y -> (x,y))

let derivative xs= let ys = Observable.timeStamp xs in
                   Observable.zip ys (Observable.skip ys 1) (fun x y -> (x.Value - y.Value) / ((x.Timestamp - y.Timestamp).TotalMilliseconds))

let vmax = Observable.rx { return 42.0 }
let maxAngleToWall = Observable.rx { return 20.0 }

let (-) = Observable.liftM2 (-)

let (*) = Observable.liftM2 (*)

let limit = 
    let limit high x = min (max (-high) x) high
    Observable.liftM2 limit

let sin = Observable.liftM sin

type FloatB = IObservable<float>

type WheelControl = IObservable<float * float>

let basicWallFollower (vel:FloatB) (side:FloatB) (front:FloatB) (setpoint:FloatB) : WheelControl = 
    let v = limit vmax (front - setpoint)
    let targetSideV = limit (vel * (sin maxAngleToWall)) (setpoint - side)
    let omega = targetSideV - (derivative side)
    pairB v omega
   
    
