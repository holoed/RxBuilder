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
            current.Robot.Position = newWorld.Robot.Position;                                    
        }

        private static World ConvertToViewModel(RxWorld.World arg)
        {
            return new World
                       {
                           Robot = new Robot
                                       {
                                           Size = new Size(arg.robot.size.Item1, arg.robot.size.Item2),
                                           Position = new Point(arg.robot.pos.Item1, arg.robot.pos.Item2)                                           
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

        private Point _position;
        public Point Position
        {
            get { return _position; }
            set {if (value == _position) return;
                _position = value;
                  OnPropertyChanged("Position");}
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
