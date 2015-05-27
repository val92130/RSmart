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

namespace Server.App.MapRendering
{
    public class DrawingMethod
    {
        public static void DrawRectangle( Canvas grid, int x, int y, int width, int height, Color color )
        {
            DrawRectangle( grid,(double)x, (double)y, (double)width, (double)height, color );
        }
        public static void DrawRectangle( Canvas grid, double x, double y, double width, double height, Color color )
        {
            Rectangle rect = new Rectangle()
            {
                Width = width,
                Height = height,
                StrokeThickness = 1
            };
            rect.Stroke = new SolidColorBrush( color );
            grid.Children.Add( rect );
            Canvas.SetTop( rect, y );
            Canvas.SetLeft( rect, x );
        }

        public static void FillRectangle( Canvas grid, int x, int y, int width, int height, Color color )
        {
            FillRectangle(grid, (double)x, (double)y, (double)width, (double)height, color );
        }
        public static void FillRectangle(Canvas grid, double x, double y, double width, double height, Color color )
        {
            Rectangle rect = new Rectangle()
            {
                Width = width,
                Height = height

            };
            rect.Fill = new SolidColorBrush( color );
            grid.Children.Add( rect );
            Canvas.SetTop( rect, y );
            Canvas.SetLeft( rect, x );
        }
    }
}
