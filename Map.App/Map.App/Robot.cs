using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Map.App
{
    public class Robot
    {
        private Vector2 _position;
        private Rectangle _area;
        private MainGame _game;
        private Vector2 _orientation;
        private float _angle;
        private DateTime now, prev = DateTime.UtcNow;
        public Robot(MainGame game,Vector2 position, int width, int height)
        {
            _orientation = new Vector2(0f, -1);
            _position = position;
            _game = game;
            _area = new Rectangle((int)_position.X, (int)_position.Y, width, height);

            GetObstacles();
        }

        public void GetObstacles()
        {
            string str = "[{\"Y\":1,\"X\":0},{\"Y\":5,\"X\":10},{\"Y\":-5,\"X\":18}]";
            try
            {
                string obstacles = _game.RobotControl.SendRequestRobot( "GetObstacles=true" );
                JArray arrObstacles = JsonConvert.DeserializeObject<JArray>( obstacles );

                for( int i = 0; i < arrObstacles.Count; i++ )
                {
                    Debug.Print( arrObstacles[i]["X"].ToString() );
                    Debug.Print( arrObstacles[i]["Y"].ToString() );

                    Point p = new Point(int.Parse(arrObstacles[i]["X"].ToString()), int.Parse(arrObstacles[i]["Y"].ToString()));

                    int mapX = Convert.ToInt32((double)p.X / _game.BoxWidth);
                    int mapY = Convert.ToInt32((double)p.Y / _game.BoxWidth);

                    _game.GetBoxAt(mapX , mapY ).IsObstacle = true;

                }

                

            }
            catch( Exception e)
            {
                
                //
            }


        }

        public Rectangle Area
        {
            get
            {
                return _area;
            }
        }

        public Vector2 Position
        {
            get
            {
                return _position;
            }
            set { _position = value; }
        }

        public int X
        {
            get { return (int)_position.X; }
            set { _position.X = value; }
        }

        public int Y
        {
            get { return (int)_position.Y; }
            set { _position.Y = value; }
        }

        public void Update()
        {
            now = DateTime.UtcNow;

            TimeSpan t = now - prev;
            if (t.TotalMilliseconds >= 3000)
            {
                prev = DateTime.UtcNow;
                GetNewPosition();
                GetObstacles();
            }
        }

        public Vector2 Orientation
        {
            get { return _orientation; }
            set { _orientation = value; }
        }

        public void Draw(SpriteBatch batch)
        {
            Texture2D text = _game.GetTexture(this);
            float ratioWidth = (float)this._area.Width / text.Width;
            float ratioHeight = (float)this._area.Height / text.Height;

            Server.Lib.Vector2 curr = new Server.Lib.Vector2(this.X + 1, this.Y + 1);
            Server.Lib.Vector2 orien = new Server.Lib.Vector2(this.Orientation.X, this.Orientation.Y);

            float angle = (float)Server.Lib.Vector2.RadianToDegree(Server.Lib.Vector2.GetAngle((curr), (orien)));

            batch.Draw(_game.GetTexture(this), Position + Orientation, null, Color.White, 0f, Vector2.Zero, new Vector2(ratioWidth, ratioHeight), SpriteEffects.None, 0f);
        }

        public void GetNewPosition()
        {
            var fmt = new NumberFormatInfo {NegativeSign = "-"};
            string x = null;
            string y = null;
            string orientationX = null;
            string orientationY = null;
            try
            {
                orientationX = _game.RobotControl.SendRequestRobot("GetOrientationX=true");
                orientationY = _game.RobotControl.SendRequestRobot( "GetOrientationX=true" );
                x = _game.RobotControl.SendRequestRobot("GetPositionX=true");
                y = _game.RobotControl.SendRequestRobot("GetPositionY=true");
            }
            catch (Exception e)
            {
                _game.RobotControl.DebugLog.Write("Error getting the position of the robot : " + e.ToString());
            }

            if (x == null || y == null || orientationY == null || orientationX == null)
            {
                return;
            }
            this.Orientation = new Vector2(float.Parse(orientationX, CultureInfo.InvariantCulture),float.Parse(orientationY, CultureInfo.InvariantCulture)) ;
            this.X = (int)(double.Parse(x, CultureInfo.InvariantCulture) );
            this.Y = (int)(double.Parse(y, CultureInfo.InvariantCulture) );
        }
    }
}
