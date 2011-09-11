using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Windows;

namespace RxWorldCSharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            RxWorld.Run(() => Tuple.Create(canvasWorld.ActualWidth, canvasWorld.ActualHeight), ConvertToViewModel).ObserveOnDispatcher().Subscribe(UpdateView());
        }

        private Action<World> UpdateView()
        {
            return x =>
                       {
                           if (DataContext == null)
                               DataContext = x;
                           else 
                               UpdateViewModel((World) DataContext, x);
                       };
        }

        private static void UpdateViewModel(World current, World newWorld)
        {
            current.Robot.Size = newWorld.Robot.Size;
            current.Robot.Position1 = newWorld.Robot.Position1;
            current.Robot.Position2 = newWorld.Robot.Position2;                                    
        }

        private static World ConvertToViewModel(RxWorld.World arg)
        {
            return new World
                       {
                           Robot = new Robot
                                       {
                                           Size = new Size(arg.robot.size.Item1, arg.robot.size.Item2),
                                           Position1 = new Point(Geometry2DLib.Vector.x(Geometry2DLib.Segment.p1(arg.robot.pos)), Geometry2DLib.Vector.y(Geometry2DLib.Segment.p1(arg.robot.pos))),                                          
                                           Position2 = new Point(Geometry2DLib.Vector.x(Geometry2DLib.Segment.p2(arg.robot.pos)), Geometry2DLib.Vector.y(Geometry2DLib.Segment.p2(arg.robot.pos)))                                           
                                       }
                       };
        }
    }

    public class Robot : INotifyPropertyChanged
    {
        private Size _size;
        public Size Size
        {
            get { return _size; }
            set {if (value == _size) return;
                  _size = value;
                  OnPropertyChanged("Size");}
        }

        private Point _position1;
        public Point Position1
        {
            get { return _position1; }
            set {if (value == _position1) return;
                _position1 = value;
                  OnPropertyChanged("Position1");}
        }

        private Point _position2;
        public Point Position2
        {
            get { return _position2; }
            set
            {
                if (value == _position2) return;
                _position2 = value;
                OnPropertyChanged("Position2");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(name));
        }
    }

    public class World 
    {     
        public Robot Robot { get; set; }           
    }
}
