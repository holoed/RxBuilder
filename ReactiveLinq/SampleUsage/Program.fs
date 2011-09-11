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

let drawLine segment color = canvas.Children.Add (new Line (X1 = Segments.p1x segment, 
                                                            Y1 = Segments.p1y segment, 
                                                            X2 = Segments.p2x segment, 
                                                            Y2 = Segments.p2y segment, 
                                                            Stroke = color, 
                                                            StrokeThickness = 5.0 )) |> ignore

let drawArrow segment color = canvas.Children.Add (new ArrowLine (X1 = Segments.p1x segment, 
                                                                  Y1 = Segments.p1y segment, 
                                                                  X2 = Segments.p2x segment, 
                                                                  Y2 = Segments.p2y segment, 
                                                                  Stroke = color, 
                                                                  StrokeThickness = 5.0 )) |> ignore

let s = Segments.create (Vectors.create 100.0 150.0) 
                       (Vectors.create 200.0 200.0)

let mp = Segments.midpoint s

let (v1, v2) = Segments.normals s

let velocity = Vectors.fromPolar 100.0 (degToRad 60.0<deg>) 

let vs1 = Segments.create mp (v1 + mp)

let vs2 = Segments.create mp (v2 + mp)

let velocityArrow = Segments.create (mp - velocity) mp 

let bounceVel = Vectors.bounce 0.5 velocity v2

let bvs = Segments.create mp (mp +  bounceVel)

let p = Vectors.create 170.0 185.0

let distanceToSegment = Segments.distanceBetweenSegmentAndPoint s p

drawLine s (Brushes.Red)

drawArrow vs1 (Brushes.Blue)

drawArrow vs2 (Brushes.Green)

drawArrow velocityArrow (Brushes.Black)

drawArrow bvs (Brushes.Coral)

drawLine (Segments.create p (p * 1.01)) (Brushes.Red)
 
let window = new Window(Title = "WpfApplication1", Content = canvas)
[<STAThread>] ignore <| (new Application()).Run window