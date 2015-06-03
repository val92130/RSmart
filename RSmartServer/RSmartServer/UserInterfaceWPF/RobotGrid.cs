using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.App
{
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using Server.App.MapRendering;
    using Server.Lib;

    public class RobotGrid
    {
        private Rect _robotPos;

        private RobotControl _robotControl;

        public RobotGrid(RobotControl r)
        {
            _robotControl = r;
            _robotPos = new Rect(0,0,24,40);          
        }

        public void DrawRobot(Canvas grid)
        {
            DrawingMethod.FillRectangle(grid,_robotPos.X, _robotPos.Y,_robotPos.Width,_robotPos.Height,Colors.Brown);
        }

        public void GetPositionRobot()
        {
            var fmt = new NumberFormatInfo();
            fmt.NegativeSign = "-";
            string x = null;
            string y = null;
            try
            {
                x = _robotControl.SendRequestRobot( "GetPositionX=true" );
                y = _robotControl.SendRequestRobot( "GetPositionY=true" );
            }
            catch (Exception e)
            {
                _robotControl.DebugLog.Write( "Error getting the position of the robot : " + e.ToString() );
            }

            if (x == null || y == null)
            {
                return;
            }
            _robotPos.X = double.Parse( x, CultureInfo.InvariantCulture ) / 10;
            _robotPos.Y = double.Parse( y, CultureInfo.InvariantCulture ) / 10;
        } 
    }
}
