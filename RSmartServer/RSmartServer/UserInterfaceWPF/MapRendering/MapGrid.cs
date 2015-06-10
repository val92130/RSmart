using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Server.Lib.MapRendering;
using System.Drawing;
using System.Windows;

namespace Server.Lib
{
    public class MapGrid
    {
        private Box[,] _boxes;
        private int _boxWidth;
        public MapGrid(int boxCountPerLine, int screenWidth, int screenHeight )
        {
            _boxWidth = 10;

            _boxes = new Box[boxCountPerLine, boxCountPerLine];

            for( int i = 0; i < boxCountPerLine; i++ )
            {
                for( int j = 0; j < boxCountPerLine; j++ )
                {
                    _boxes[i, j] = new Box(i * _boxWidth, j * _boxWidth, _boxWidth);
                }
            }
        }

        public void Draw(Canvas c)
        {
            foreach (Box b in _boxes)
            {
                b.Draw(c);
            }
        }

        public Box this[int i, int j]
        {
            get { return _boxes[i, j]; }
        }

        public List<Box> GetOverlappedBoxes(Rect r)
        {
            var boxList = new List<Box>();
            int top = (int)(r.Top / this._boxWidth);
            int left = (int)(r.Left / this._boxWidth);
            int bottom = (int)((r.Bottom - 1) / this._boxWidth);
            int right = (int)((r.Right - 1) / this._boxWidth);
            for (int i = top; i <= bottom; ++i)
            {
                for (int j = left; j <= right; ++j)
                {
                    if (this[i, j] != null)
                    {
                        Box b = this[j, i];
                        boxList.Add(b);
                    }
                }
            }

            return boxList;
        }

        public Box[,] Boxes
        {
            get
            {
                return _boxes;
            }
        }
    }
}
