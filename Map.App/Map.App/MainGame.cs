using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Server.Lib;
namespace Map.App
{
    public class MainGame
    {
        private Camera2d _cam;
        private int mapWidth;
        private Box[,] _boxes;
        private const int BoxWidthCm = 20;
        private Texture2D _collisionTexture, _boxTexture, _robotTexture;
        private Game1 _game;
        private int _windowsWidth, _winwowsHeight;
        private Robot _robot;
        private readonly RobotControl _robotControl;
        private int boxCountPerLine;
        public MainGame(int widthCm, Game1 game)
        {
            _robotControl = new RobotControl(); 
            _robot = new Robot(this, Microsoft.Xna.Framework.Vector2.Zero,28,30 );
            _game = game;
            _windowsWidth = _game.Window.ClientBounds.Width;
            _winwowsHeight = _game.Window.ClientBounds.Height;
            _cam = new Camera2d
            {
                Pos = new Microsoft.Xna.Framework.Vector2( 0.0f, 0.0f ),
                Zoom = 1f
            };
            mapWidth = widthCm;
            _boxes = new Box[mapWidth / BoxWidthCm, mapWidth / BoxWidthCm];

            boxCountPerLine = mapWidth/BoxWidthCm;

            for (int i = 0; i < boxCountPerLine; i++)
            {
                for (int j = 0; j < boxCountPerLine; j++)
                {
                    _boxes[i, j] = new Box(this,i * BoxWidthCm, j * BoxWidthCm, BoxWidthCm, BoxWidthCm);
                }
            }

        }

        public void Update(GameTime gameTime)
        {
            _robot.Update(gameTime);
        }


        public void LoadContent(ContentManager content)
        {
            _collisionTexture = content.Load<Texture2D>("collision");
            _boxTexture = content.Load<Texture2D>("floor");
            _robotTexture = content.Load<Texture2D>("robot");
        }

        public Texture2D GetTexture(Robot r)
        {
            return _robotTexture;
        }

        public RobotControl RobotControl
        {
            get { return _robotControl; }
        }
        public Camera2d Camera
        {
            get
            {
                return _cam;
            }
        }

        public void OnLeftClick()
        {
            foreach (Box b in GetOverlappedBoxes())
            {
                if (b.Area.Intersects(MouseArea))
                {
                    b.IsObstacle = true;
                }
            }
        }

        public void OnRightClick()
        {
            foreach (Box b in GetOverlappedBoxes())
            {
                if (b.Area.Intersects(MouseArea))
                {
                    b.IsObstacle = false;
                }
            }
        }

        public List<Box> GetOverlappedBoxes()
        {
            return GetOverlappedBoxes(new Rectangle((int)_cam.Pos.X, (int)_cam.Pos.Y, _windowsWidth, _winwowsHeight));
        }

        public Rectangle MouseArea
        {
            
            get
            {
                MouseState ms = Mouse.GetState();
                Microsoft.Xna.Framework.Vector2 pos = _cam.ScreenToWorld( new Microsoft.Xna.Framework.Vector2( ms.X, ms.Y ) );
                return new Rectangle((int)pos.X, (int)pos.Y, 5, 5);
            }
        }

        public Box this[int x, int y]
        {
            get
            {
                if (x < 0 || y < 0 || x >= boxCountPerLine || y >= boxCountPerLine)
                    return null;

               return _boxes[x, y];
            }
        }

        public List<Box> GetOverlappedBoxes(Rectangle r)
        {
            var boxList = new List<Box>();
            int top = r.Top / BoxWidthCm;
            int left = r.Left / BoxWidthCm;
            int bottom = (r.Bottom - 1) / BoxWidthCm;
            int right = (r.Right - 1) / BoxWidthCm;
            for (int i = top; i <= bottom; ++i)
            {
                for (int j = left; j <= right; ++j)
                {
                    if (this[i, j] != null)
                    {
                        Box b = this[j, i];
                        if (b == null)
                            continue;
                        boxList.Add(b);
                    }
                }
            }

            return boxList;
        }


        public Texture2D GetTexture(Box b)
        {
            if (b.IsObstacle)
            {
                return _collisionTexture;
            }

            return _boxTexture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            List<Box> boxes = GetOverlappedBoxes(new Rectangle((int)_cam.Pos.X, (int)_cam.Pos.Y, (int)(_windowsWidth / _cam.Zoom), (int)(_winwowsHeight / _cam.Zoom)));

            foreach (Box b in boxes)
            {
                b.Draw(spriteBatch);
            }

            _robot.Draw(spriteBatch);
        }
    }
}
