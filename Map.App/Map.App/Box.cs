using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Map.App
{
    public class Box
    {
        private Rectangle _area;
        private Vector2 _position;
        private bool _isObstacle = false;
        private MainGame _game;
        public Box(MainGame game,int x, int y, int width, int height)
        {
            _game = game;
            _position = new Vector2(x,y);
            _area = new Rectangle((int)_position.X, (int)_position.Y, width, height);
        }

        public Box(MainGame game,int x, int y, int width, int height, bool isObstacle)
            : this(game, x, y, width, height)
        {
            _isObstacle = true;
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
            get { return (int) _position.X; }
            set { _position.X = value; }
        }

        public int Y
        {
            get { return (int)_position.Y; }
            set { _position.Y = value; }
        }

        public bool IsObstacle
        {
            get
            {
                return _isObstacle;
            }
            set { _isObstacle = value; }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_game.GetTexture(this), this.Area, Color.White);
        }
    }
}
