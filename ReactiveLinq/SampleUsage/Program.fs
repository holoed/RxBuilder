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

let drawLine segment color = canvas.Children.Add (new Line (X1 = Segment.p1x segment, 
                                                            Y1 = Segment.p1y segment, 
                                                            X2 = Segment.p2x segment, 
                                                            Y2 = Segment.p2y segment, 
                                                            Stroke = color, 
                                                            StrokeThickness = 5.0 )) |> ignore

let drawArrow segment color = canvas.Children.Add (new ArrowLine (X1 = Segment.p1x segment, 
                                                                  Y1 = Segment.p1y segment, 
                                                                  X2 = Segment.p2x segment, 
                                                                  Y2 = Segment.p2y segment, 
                                                                  Stroke = color, 
                                                                  StrokeThickness = 5.0 )) |> ignore

let s = Segment.create (Vector.create 100.0 150.0) 
                       (Vector.create 200.0 200.0)

let mp = Segment.midpoint s

let (v1, v2) = Segment.normals s

let velocity = Vector.fromPolar 100.0 (degToRad 60.0<deg>) 

let vs1 = Segment.create mp (v1 + mp)

let vs2 = Segment.create mp (v2 + mp)

let velocityArrow = Segment.create (mp - velocity) mp 

let bounceVel = Vector.bounce velocity v2

let bvs = Segment.create mp (mp +  bounceVel)

let p = Vector.create 170.0 185.0

let distanceToSegment = Segment.distanceBetweenSegmentAndPoint s p

drawLine s (Brushes.Red)

drawArrow vs1 (Brushes.Blue)

drawArrow vs2 (Brushes.Green)

drawArrow velocityArrow (Brushes.Black)

drawArrow bvs (Brushes.Coral)

drawLine (Segment.create p (p * 1.01)) (Brushes.Red)
 
let window = new Window(Title = "WpfApplication1", Content = canvas)
[<STAThread>] ignore <| (new Application()).Run window