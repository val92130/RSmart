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
        DispatcherTimer t;
        private MapGrid _map;
        private Rect _mouseRect;
        private bool add = true;
        public Map(RobotControl r)
        {     
            _robotControl = r;
            InitializeComponent();
            _map = new MapGrid( 20, (int)this.Width, (int)this.Height );
            t = new DispatcherTimer {Interval = new TimeSpan(0, 0, 0, 0, 10)};
            t.Tick += new EventHandler(T_Tick);
            t.Start();
            
        }

        // Draw
        private void T_Tick(object sender, EventArgs e)
        {

            grid.Children.Clear();
            _map.Draw( grid );

            _mouseRect = new Rect( Mouse.GetPosition( this ), new Size( 10, 10 ) );
            DrawingMethod.DrawRectangle( grid, _mouseRect.X, _mouseRect.Y, _mouseRect.Width, _mouseRect.Height, Colors.Blue );
            Update();
        }

        public void Update()
        {
            foreach (Box b in _map.Boxes)
            {
                if (_mouseRect.IntersectsWith(b.Area))
                {
                    DrawingMethod.DrawRectangle( grid, b.Area.X, b.Area.Y, b.Area.Width, b.Area.Height, Colors.Black );
                }
            } 
            
        }


        private void Grid_MouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            if (add)
            {
                foreach (Box b in _map.Boxes)
                {
                    if (_mouseRect.IntersectsWith(b.Area))
                    {
                        b.IsObstacle = true;
                    }
                }
            }
            else
            {
                foreach( Box b in _map.Boxes )
                {
                    if( _mouseRect.IntersectsWith( b.Area ) )
                    {
                        b.IsObstacle = false;
                    }
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
