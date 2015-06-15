using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Server.Lib;
using Vector2 = Microsoft.Xna.Framework.Vector2;

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
        private bool _exited = false;
        private List<Button> _buttons;
        public delegate void ClickHandler(object sender);
        public EControlMode _controlMode = EControlMode.AddObstacle;
        List<Vector2> _points = new List<Vector2>();
        private Texture2D texture, rectText;
        private Texture2D circle;

        private List<Texture2D> _circles = new List<Texture2D>();
        
        public MainGame(int widthCm, Game1 game)
        {
            _buttons = new List<Button>();
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

            Thread updateThread = new Thread(() =>
            {
                while (!_exited)
                {
                    _robot.Update();
                }
                
            });


            updateThread.Start();

            _buttons.Add(new Button(new Microsoft.Xna.Framework.Vector2(10,10),"Add Points", 200,30,Color.Brown, this, new ClickHandler(ChangeModeButtonClick) ));
            _buttons.Add( new Button( new Microsoft.Xna.Framework.Vector2( 10, 60 ), "Add Obstacles", 200, 30, Color.Brown, this, new ClickHandler( AddObstaclesButtonClick ) ) );


        }

        private void AddObstaclesButtonClick( object sender )
        {
            _controlMode = EControlMode.AddObstacle;
            Debug.Print( "Add obstacles mode : " + sender.ToString() );
        }

        private void ChangeModeButtonClick(Object sender)
        {
            _controlMode = EControlMode.AddPoints;
            Debug.Print("Add points mode : " + sender.ToString());
        }

        public void Update(GameTime gameTime)
        {
            foreach (Button b in _buttons)
            {
                b.Update();
            }
        }


        public void LoadContent(ContentManager content, GraphicsDevice graphics)
        {
            _collisionTexture = content.Load<Texture2D>("collision");
            _boxTexture = content.Load<Texture2D>("floor");
            _robotTexture = content.Load<Texture2D>("robot");

            texture = new Texture2D( graphics, 1, 1, false, SurfaceFormat.Color );
            texture.SetData<Color>( new Color[] { Color.Black } );

            rectText = new Texture2D( graphics, 1, 1, false, SurfaceFormat.Color );
            rectText.SetData<Color>( new Color[] { Color.Blue } );


            

            foreach (Button b in _buttons)
            {
                b.LoadContent( graphics );
            }


            double radius = Server.Lib.Vector2.Radius(new Server.Lib.Vector2(1, 1),
                new Server.Lib.Vector2(_robot.Orientation.X, _robot.Orientation.Y), new Server.Lib.Vector2(100, 200));

            circle = CreateCircle((int)radius);
        }

        public void Draw( SpriteBatch spriteBatch )
        {
            
            List<Box> boxes = GetOverlappedBoxes( new Rectangle( (int)_cam.Pos.X, (int)_cam.Pos.Y, (int)(_windowsWidth / _cam.Zoom), (int)(_winwowsHeight / _cam.Zoom) ) );

            foreach( Box b in boxes )
            {
                b.Draw( spriteBatch );
            }

            _robot.Draw( spriteBatch );


            for(int i = 0; i < _points.Count; i++)
            {
                this.DrawLine(spriteBatch,i == 0 ? _robot.Position : _points[i - 1], _points[i] , Color.Black);
                DrawPoint( _points[i], spriteBatch );
            }

            DrawPoint( new Vector2( 100, 200 ), spriteBatch);
            DrawPoint(new Vector2(circle.Width / 2 + 100, circle.Height / 2 + 200) , spriteBatch );
            spriteBatch.Draw( circle, new Vector2(  - (circle.Width) + 100, 200 - (circle.Height/2) ), Color.Red );
            
            
        }

        public void DrawPoint(Vector2 position, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw( rectText, position, new Rectangle( (int)position.X + 3,(int) position.Y + 3, 6, 6 ), Color.White );
        }

        public Texture2D CreateCircle( int radius )
        {
            int outerRadius = radius * 2 + 2; // So circle doesn't go out of bounds
            Texture2D texture = new Texture2D( _game.GraphicsDevice, outerRadius, outerRadius );

            Color[] data = new Color[outerRadius * outerRadius];

            // Colour the entire texture transparent first.
            for( int i = 0; i < data.Length; i++ )
                data[i] = Color.Transparent;

            // Work out the minimum step necessary using trigonometry + sine approximation.
            double angleStep = 1f / radius;

            for( double angle = 0; angle < Math.PI * 2; angle += angleStep )
            {
                // Use the parametric definition of a circle:
                int x = (int)Math.Round( radius + radius * Math.Cos( angle ) );
                int y = (int)Math.Round( radius + radius * Math.Sin( angle ) );

                data[y * outerRadius + x + 1] = Color.White;
            }

            texture.SetData( data );
            return texture;
        }


        public void DrawLine( SpriteBatch spriteBatch, Vector2 begin, Vector2 end, Color color, int width = 1 )
        {
            Rectangle r = new Rectangle( (int)begin.X, (int)begin.Y, (int)(end - begin).Length() + width, width );
            Vector2 v = Vector2.Normalize( begin - end );
            float angle = (float)Math.Acos( Vector2.Dot( v, -Vector2.UnitX ) );
            if( begin.Y > end.Y ) angle = MathHelper.TwoPi - angle;
            spriteBatch.Draw( texture, r, null, color, angle, Vector2.Zero, SpriteEffects.None, 0 );
        }
        public void DrawGUI(SpriteBatch spriteBatch, SpriteFont font)
        {
            foreach( Button b in _buttons )
            {
                b.Draw( spriteBatch, font );
            }
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
            MouseState m = Mouse.GetState();
            
            foreach( Button b in _buttons )
            {
                if( b.Area.Contains( new Point( m.X, m.Y ) ) )
                {
                    return;
                }
            }

            switch (_controlMode)
            {
                case EControlMode.AddObstacle:
                    foreach( Box b in GetOverlappedBoxes() )
                    {
                        if( b.Area.Intersects( MouseArea ) )
                        {
                            this.AddObstacleRobot( b.Position );
                            b.IsObstacle = true;
                        }
                    }
                    break;
                case EControlMode.AddPoints:
                    // TODO
                    _points.Add(_cam.ScreenToWorld(new Vector2(m.X, m.Y)));
                    break;
            }
            
            
        }

        public void AddObstacleRobot(Microsoft.Xna.Framework.Vector2 position)
        {
            _robotControl.SendRequestRobot("AddObstacle=" + position.X + ";" + position.Y);
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



        internal void OnExit( object sender, EventArgs e )
        {
            _exited = true;
        }
    }
}
