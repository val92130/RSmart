using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Server.App.MapRendering;

namespace Server.Lib.MapRendering
{
    public class Box
    {
        private int x, y, width;
        private Color _color = Colors.Red;
        private Rect _area;
        private bool isObstacle = false;
        public Box(int x, int y, int width)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            _area = new Rect(x,y,width,width);
        }

        public Rect Area
        {
            get
            {
                return _area;
            }
        }

        public bool IsObstacle
        {
            get { return isObstacle; }
            set { isObstacle = value; }
        }
        public void Draw(Canvas c)
        {
            if (isObstacle)
            {
                DrawingMethod.DrawRectangle( c, x, y, width, width, Colors.White );
                DrawingMethod.FillRectangle( c, x, y, width, width, Colors.Black );
            }
            else
            {
                DrawingMethod.DrawRectangle( c, x, y, width, width, Colors.BlueViolet );
            }
           
        }
    }
}
