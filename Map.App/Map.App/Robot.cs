using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
            _orientation = new Vector2(0, -1);
            _position = position;
            _game = game;
            _area = new Rectangle((int)_position.X, (int)_position.Y, width, height);
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

        public void Update(GameTime gameTime)
        {
            now = DateTime.UtcNow;

            TimeSpan t = now - prev;
            if (t.TotalMilliseconds >= 3000)
            {
                prev = DateTime.UtcNow;
                GetNewPosition();
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
