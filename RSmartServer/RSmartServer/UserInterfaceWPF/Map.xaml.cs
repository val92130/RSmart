using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Server.App.MapRendering;
using Server.Lib;
using Server.Lib.MapRendering;

namespace Server.App
{
    /// <summary>
    /// Interaction logic for Map.xaml
    /// </summary>
    public partial class Map : Window
    {
        private RobotControl _robotControl;
        DispatcherTimer t,updateTimer;
        private MapGrid _map;
        private Rect _mouseRect;
        private bool add = true;

        private RobotGrid _robotGrid;
        public Map(RobotControl r)
        {     
            _robotControl = r;
           _robotGrid = new RobotGrid(r);
            InitializeComponent();
            _map = new MapGrid( 100, (int)this.Width, (int)this.Height );
            t = new DispatcherTimer {Interval = new TimeSpan(0, 0, 0, 0, 10)};
            t.Tick += new EventHandler(T_Tick);
            t.Start();
            updateTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, 1000) };
            updateTimer.Tick += new EventHandler(T_UpdateRobot);
            updateTimer.Start();
            
        }

        private void T_UpdateRobot(object sender, EventArgs e)
        {
            _robotGrid.GetPositionRobot();
        }

        // Draw
        private void T_Tick(object sender, EventArgs e)
        {
            grid.Children.Clear();
            _map.Draw( grid );

            _mouseRect = new Rect( Mouse.GetPosition( this ), new Size( 10, 10 ) );
            DrawingMethod.DrawRectangle( grid, _mouseRect.X, _mouseRect.Y, _mouseRect.Width, _mouseRect.Height, Colors.Blue );
           _robotGrid.DrawRobot(grid);
            Update();
        }

        public void Update()
        {
             DrawingMethod.DrawRectangle(grid, _mouseRect.X, _mouseRect.Y, _mouseRect.Width, _mouseRect.Height, Colors.Black);
        }


        private void Grid_MouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            if (add)
            {
                foreach (Box b in _map.GetOverlappedBoxes(_mouseRect))
                {
                    b.IsObstacle = true;
                }
            }
            else
            {
                foreach (Box b in _map.GetOverlappedBoxes(_mouseRect))
                {
                    b.IsObstacle = false;
                }
            }
            
        }

        private void addObstacleButton_Click( object sender, RoutedEventArgs e )
        {
            add = true;
        }

        private void removeObstacleButton_Click( object sender, RoutedEventArgs e )
        {
            add = false;
        }




    }
}
