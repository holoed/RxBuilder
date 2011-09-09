
// Learn more about F# at http://fsharp.net
// F# for WPF project template at http://code.msdn.com/fs
// Template Version 0.1 Preview
open System
open System.Windows
open System.Windows.Controls
open System.Windows.Shapes
open System.Windows.Media
open Petzold.Media2D
open MathLib
open Geometry2DLib

let canvas = new Canvas()

let drawLine segment color = canvas.Children.Add (new Line (X1 = getP1X segment, 
                                                            Y1 = getP1Y segment, 
                                                            X2 = getP2X segment, 
                                                            Y2 = getP2Y segment, 
                                                            Stroke = color, 
                                                            StrokeThickness = 5.0 )) |> ignore

let drawArrow segment color = canvas.Children.Add (new ArrowLine (X1 = getP1X segment, 
                                                                  Y1 = getP1Y segment, 
                                                                  X2 = getP2X segment, 
                                                                  Y2 = getP2Y segment, 
                                                                  Stroke = color, 
                                                                  StrokeThickness = 5.0 )) |> ignore

let s = segment (vector 100.0 150.0) (vector 200.0 200.0)

let mp = midpoint s

let (v1, v2) = normals s

let velocity = fromPolar 100.0 (toRad 60.0<deg>) 

let vs1 = segment mp (v1 + mp)

let vs2 = segment mp (v2 + mp)

let velocityArrow = segment (mp - velocity) mp 

let bounceVel = bounce velocity v2

let bvs = segment mp (mp +  bounceVel)

let p = vector 170.0 185.0

let distanceToSegment = distanceBetweenSegmentAndPoint s p

drawLine s (Brushes.Red)

drawArrow vs1 (Brushes.Blue)

drawArrow vs2 (Brushes.Green)

drawArrow velocityArrow (Brushes.Black)

drawArrow bvs (Brushes.Coral)

drawLine (segment p (p * 1.01)) (Brushes.Red)
 
let window = new Window(Title = "WpfApplication1", Content = canvas)
[<STAThread>] ignore <| (new Application()).Run window