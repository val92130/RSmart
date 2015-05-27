using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Lib.MapRendering;

namespace Server.Lib
{
    public class MapGrid
    {
        private int _boxWidth;
        private Box[,] _boxes;
        public MapGrid(int boxCountPerLine, int screenWidth, int screenHeight )
        {
            _boxWidth = screenWidth/boxCountPerLine;

            _boxes = new Box[boxCountPerLine, boxCountPerLine];

            for( int i = 0; i < boxCountPerLine; i++ )
            {
                for( int j = 0; j < boxCountPerLine; j++ )
                {
                    _boxes[i,j] = new Box(i * _boxWidth, j * _boxWidth, _boxWidth);
                }
            }
        }

        public void Draw()
        {
            foreach (Box b in _boxes)
            {
                b.Draw();
            }
        }
    }
}
