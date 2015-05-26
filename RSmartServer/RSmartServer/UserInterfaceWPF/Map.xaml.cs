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
using Server.Lib;

namespace Server.App
{
    /// <summary>
    /// Interaction logic for Map.xaml
    /// </summary>
    public partial class Map : Window
    {
        private RobotControl _robotControl;
        DispatcherTimer t;
        public Map(RobotControl r)
        {
            _robotControl = r;
            InitializeComponent();
            t = new DispatcherTimer {Interval = new TimeSpan(0, 0, 0, 0, 10)};
            t.Tick += new EventHandler(T_Tick);
            t.Start();
            DrawGrid(50, 50, 0.24);
        }

        // Draw
        private void T_Tick(object sender, EventArgs e)
        {
            //grid.Children.Clear();
        }

        private void DrawGrid(double gridWidthMeters, double gridHeightMeters, double boxWidth)
        {
            int ratio = 10;
            for (double i = 0; i < gridWidthMeters * ratio; i += boxWidth * ratio)
            {
                for (double j = 0; j < gridHeightMeters * ratio; j += boxWidth * ratio)
                {
                    DrawRectangle(i, j, boxWidth * ratio, boxWidth * ratio, Colors.Blue);
                }
            }
        }

        public void DrawRectangle(int x, int y, int width, int height, Color color)
        {
            DrawRectangle((double)x, (double)y, (double)width, (double)height, color);
        }
        public void DrawRectangle(double x, double y, double width, double height, Color color)
        {
            Rectangle rect = new Rectangle()
            {
                Width = width,
                Height = height,
                StrokeThickness = 1
            };
            rect.Stroke = new SolidColorBrush(color);
            grid.Children.Add(rect);
            Canvas.SetTop(rect, y);
            Canvas.SetLeft(rect, x);
        }

        public void FillRectangle(int x, int y, int width, int height, Color color)
        {
            FillRectangle((double)x, (double)y, (double)width, (double)height, color);
        }
        public void FillRectangle(double x, double y, double width, double height, Color color)
        {
            Rectangle rect = new Rectangle()
            {
                Width = width,
                Height = height

            };
            rect.Fill = new SolidColorBrush(color);
            grid.Children.Add(rect);
            Canvas.SetTop(rect, y);
            Canvas.SetLeft(rect, x);
        }

    }
}
