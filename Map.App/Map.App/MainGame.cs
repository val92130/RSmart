
 ﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Server.Lib;
using TomShane.Neoforce.Controls;
using EventArgs = System.EventArgs;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Map.App
{
    public class MainGame
    {
        private Camera2d _cam;
        private int mapWidth;
        private Box[,] _boxes;
        private const int BoxWidthCm = 10;
        private Texture2D _collisionTexture, _boxTexture, _robotTexture;
        private Game1 _game;
        private int _windowsWidth, _winwowsHeight;
        private Robot _robot;
        private readonly RobotControl _robotControl;
        private int boxCountPerLine;
        private bool _exited = false;
        private SpriteFont _font;

        public readonly int BoxWidth = 10;

        public delegate void ClickHandler(object sender);
        public EControlMode _controlMode = EControlMode.AddObstacle;
        List<DestinationPoint> _points = new List<DestinationPoint>();
        private Texture2D _texturePoint, _textureCircle;
        private ButtonManager _buttonManager;
        private List<Texture2D> _circles = new List<Texture2D>();

        private WindowManager _windowManager;

        public MainGame(int widthCm, Game1 game, int resolutionWidth, int resolutionHeight)
        {

            _buttonManager = new ButtonManager(this);
            _robotControl = new RobotControl();
            _robot = new Robot(this, new Vector2(0, 0), 28, 30);
            _game = game;
            _windowsWidth = resolutionWidth;
            _winwowsHeight = resolutionHeight;
            _cam = new Camera2d
            {
                Pos = new Microsoft.Xna.Framework.Vector2(0.0f, 0.0f),
                Zoom = 1f
            };
            mapWidth = widthCm;
            _boxes = new Box[mapWidth / BoxWidthCm, mapWidth / BoxWidthCm];

            boxCountPerLine = mapWidth / BoxWidthCm;

            for (int i = 0; i < boxCountPerLine; i++)
            {
                for (int j = 0; j < boxCountPerLine; j++)
                {
                    _boxes[i, j] = new Box(this, i * BoxWidthCm, j * BoxWidthCm, BoxWidthCm, BoxWidthCm);
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

            _buttonManager.Add(new Button(new Microsoft.Xna.Framework.Vector2(10, 10), "Add Points", 200, 30, new Color(103, 128, 159), this, new ClickHandler(ChangeModeButtonClick)));
            _buttonManager.Add(new Button(new Microsoft.Xna.Framework.Vector2(10, 60), "Add Obstacles", 200, 30, new Color(224, 130, 131), this, new ClickHandler(AddObstaclesButtonClick)));
            _buttonManager.Add(new Button(new Microsoft.Xna.Framework.Vector2(10, 110), "Send Points", 200, 30, new Color(134, 126, 213), this, new ClickHandler(SendDataRobotButtonClick)));
            _buttonManager.Add(new Button(new Microsoft.Xna.Framework.Vector2(10, 170), "Clear", 200, 30, new Color(245, 171, 53), this, new ClickHandler(ClearMapClick)));
            _buttonManager.Add(new Button(new Microsoft.Xna.Framework.Vector2(10, 230), "Save Path", 200, 30, new Color(190, 140, 100), this, new ClickHandler(SavePathClick)));
            _buttonManager.Add(new Button(new Microsoft.Xna.Framework.Vector2(10, 290), "Load Path", 200, 30, new Color(210, 160, 80), this, new ClickHandler(LoadPathClick)));

        }

        private void LoadPathClick(object sender)
        {
            _windowManager.ShowLoadPathWindow((new Rectangle(0, 0, 300, 200)));
        }

        public void LoadPoints(List<DestinationPoint> pointList)
        {
            _points = pointList;
        }

        private void SavePathClick(object sender)
        {
            _windowManager.ShowSavePathWindow(new Rectangle(0, 0, 300, 200));
        }

        public void Initialize(Game game, GraphicsDeviceManager graphics)
        {
            _windowManager = new WindowManager(this, game, graphics);
            _windowManager.Initialize();
        }

        public static string ValidateString(string s)
        {
            string str = s;
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                str = str.Replace(c, '_');
            }

            return str;
        }

        public void SavePath(string filename)
        {
            string name = ValidateString(filename);
            try
            {
                using (Stream stream = File.Open(name + ".path", FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, _points);
                }
            }
            catch (IOException)
            {

            }
        }

        public WindowManager WindowManager
        {
            get
            {
                return _windowManager;
            }
        }

        public Box[,] Boxes
        {
            get { return _boxes; }
        }

        private void ClearMapClick(object sender)
        {
            _robot.Position = Vector2.Zero;
            _robot.Orientation = new Vector2(0, 1);
            _points = new List<DestinationPoint>();
        }

        public List<PathInformation> GetPathList()
        {
            List<PathInformation> pathList = new List<PathInformation>();

            Vector2 robotPosition = new Vector2(_robot.Position.X, _robot.Position.Y);

            Vector2 currentOrientation = new Vector2(_robot.Orientation.X, _robot.Orientation.Y);

            foreach (DestinationPoint p in _points)
            {

                double angle = GetAngle(robotPosition, currentOrientation, p.Position);


                float oX = currentOrientation.X;
                float oY = currentOrientation.Y;
                float dX = p.Position.X;
                float dY = p.Position.Y;

                double ang;
                Vector2 VecToTarget = robotPosition - p.Position;
                if ((VecToTarget.X * currentOrientation.Y) > (VecToTarget.Y * currentOrientation.X))
                {
                    ang = angle;
                    currentOrientation = TransformPoint(currentOrientation,
                        (float)(Server.Lib.Vector2.DegreeToRadian(angle)));

                    _robot.Orientation = currentOrientation;
                }
                else
                {
                    ang = -angle;
                    currentOrientation = TransformPoint(currentOrientation,
                        -(float)(Server.Lib.Vector2.DegreeToRadian(angle)));
                    _robot.Orientation = currentOrientation;
                }

                PathInformation pathInfo = new PathInformation(ang, GetTimeToDestinationMilli(robotPosition, p.Position), new Vector2(currentOrientation.X, currentOrientation.Y));
                pathList.Add(pathInfo);
                robotPosition = p.Position;

            }

            return pathList;
        }



        private void SendDataRobotButtonClick(object sender)
        {
            string queryRobot = "";

            foreach (DestinationPoint d in _points)
            {
                queryRobot += Math.Round(d.Position.X, 2) + ":" + Math.Round(d.Position.Y, 2) + ";";
            }

            _robotControl.SendRequestRobot("SendPoints=" + queryRobot);
            _points = new List<DestinationPoint>();

            return;
            string query = "";
            foreach (PathInformation p in GetPathList())
            {
                query += (int)p.Angle + ":" + p.DurationMilli + ":" + Math.Round(p.Orientation.X, 3) + "_" + Math.Round(p.Orientation.Y, 3) +
                           ";";
            }

            _robotControl.SendRequestRobot("FollowPath=" + query);

        }

        public int GetTimeToDestinationMilli(Vector2 position, Vector2 destination)
        {
            double speedCm = 46;

            double length = Vector2.Distance(position, destination);

            double dist = length / speedCm;
            return (int)dist * 1000;

        }

        public double GetAngle(Vector2 position, Vector2 orientation, Vector2 destination)
        {
            Server.Lib.Vector2 directionVector = new Server.Lib.Vector2(destination.X - position.X, destination.Y - position.Y);
            double angle = Server.Lib.Vector2.GetAngle(new Server.Lib.Vector2(orientation.X, orientation.Y), new Server.Lib.Vector2(directionVector.X, directionVector.Y));
            return Server.Lib.Vector2.RadianToDegree(angle);
        }

        public Vector2 TransformPoint(Vector2 point, float angleRadian)
        {
            Vector2 newPoint = new Vector2(point.X, point.Y);

            float x = newPoint.X;
            float y = newPoint.Y;

            float px = (float)(x * System.Math.Cos(angleRadian) - y * System.Math.Sin(angleRadian));
            float py = (float)(x * System.Math.Sin(angleRadian) + y * System.Math.Cos(angleRadian));

            return new Vector2(px, py);
        }

        private void AddObstaclesButtonClick(object sender)
        {
            _controlMode = EControlMode.AddObstacle;
            Debug.Print("Add obstacles mode : " + sender.ToString());
        }

        private void ChangeModeButtonClick(Object sender)
        {
            _controlMode = EControlMode.AddPoints;
            Debug.Print("Add points mode : " + sender.ToString());
        }

        public void Update(GameTime gameTime)
        {
            _windowManager.Update(gameTime);
            _buttonManager.Update();
        }


        public void LoadContent(ContentManager content, GraphicsDevice graphics)
        {
            _collisionTexture = content.Load<Texture2D>("collision");
            _boxTexture = content.Load<Texture2D>("floor");
            _robotTexture = content.Load<Texture2D>("robot");

            _buttonManager.LoadContent(content, graphics);


            _texturePoint = new Texture2D(graphics, 1, 1, false, SurfaceFormat.Color);
            _texturePoint.SetData<Color>(new Color[] { Color.Blue });

            _textureCircle = new Texture2D(graphics, 1, 1, false, SurfaceFormat.Color);
            _textureCircle.SetData<Color>(new Color[] { Color.Red });


            _font = content.Load<SpriteFont>("SpriteFont1");

            double radius = Server.Lib.Vector2.Radius(new Server.Lib.Vector2(1, 1),
                new Server.Lib.Vector2(_robot.Orientation.X, _robot.Orientation.Y), new Server.Lib.Vector2(100, 200));
        }

        public SpriteFont Font
        {
            get { return _font; }
        }

        public Robot Robot
        {
            get
            {
                return _robot;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            List<Box> boxes = GetOverlappedBoxes(new Rectangle((int)_cam.Pos.X, (int)_cam.Pos.Y, (int)(_windowsWidth / _cam.Zoom), (int)(_winwowsHeight / _cam.Zoom)));


            

            foreach (Box b in boxes)
            {
                b.Draw(spriteBatch);
            }

            _robot.Draw(spriteBatch);


            for (int i = 0; i < _points.Count; i++)
            {
                _points[i].Draw(spriteBatch, _texturePoint, _textureCircle);
            }

            //DrawPoint( new Vector2( 100, 200 ), spriteBatch);
            //DrawPoint(new Vector2(circle.Width / 2 + 100, circle.Height / 2 + 200) , spriteBatch );
            //spriteBatch.Draw( circle, new Vector2(  - (circle.Width) + 100, 200 - (circle.Height/2) ), Color.Red );


            
            if (_points.Count != 0)
            {
                DrawLine(spriteBatch, _points[_points.Count - 1].Position,
                    Camera.ScreenToWorld(new Vector2(Mouse.GetState().X, Mouse.GetState().Y)), Color.Red, _texturePoint);

                spriteBatch.DrawString(_font, Math.Round(Vector2.Distance(_points[_points.Count - 1].Position, Camera.ScreenToWorld(new Vector2(Mouse.GetState().X, Mouse.GetState().Y))),2).ToString(), Camera.ScreenToWorld(new Vector2(Mouse.GetState().X + 50, Mouse.GetState().Y)), Color.Black) ;
            }
            else
            {
                DrawLine(spriteBatch, _robot.Position,
                    Camera.ScreenToWorld(new Vector2(Mouse.GetState().X, Mouse.GetState().Y)), Color.Red, _texturePoint);

                spriteBatch.DrawString(_font, Math.Round(Vector2.Distance(_robot.Position, Camera.ScreenToWorld(new Vector2(Mouse.GetState().X, Mouse.GetState().Y))),2).ToString(), Camera.ScreenToWorld(new Vector2(Mouse.GetState().X + 50, Mouse.GetState().Y)), Color.Black);
            }


        }

        public void DrawPoint(Vector2 position, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texturePoint, position, new Rectangle((int)position.X + 3, (int)position.Y + 3, 6, 6), Color.White);
        }

        public Texture2D CreateCircle(int radius)
        {
            int outerRadius = radius * 2 + 2; // So circle doesn't go out of bounds
            Texture2D texture = new Texture2D(_game.GraphicsDevice, outerRadius, outerRadius);

            Color[] data = new Color[outerRadius * outerRadius];

            // Colour the entire texture transparent first.
            for (int i = 0; i < data.Length; i++)
                data[i] = Color.Transparent;

            double angleStep = 1f / radius;

            for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
            {
                int x = (int)Math.Round(radius + radius * Math.Cos(angle));
                int y = (int)Math.Round(radius + radius * Math.Sin(angle));

                data[y * outerRadius + x + 1] = Color.White;
            }

            texture.SetData(data);
            return texture;
        }


        public static void DrawLine(SpriteBatch spriteBatch, Vector2 begin, Vector2 end, Color color, Texture2D texture, int width = 1)
        {
            Rectangle r = new Rectangle((int)begin.X, (int)begin.Y, (int)(end - begin).Length() + width, width);
            Vector2 v = Vector2.Normalize(begin - end);
            float angle = (float)Math.Acos(Vector2.Dot(v, -Vector2.UnitX));
            if (begin.Y > end.Y) angle = MathHelper.TwoPi - angle;
            spriteBatch.Draw(texture, r, null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }
        public void DrawGUI(SpriteBatch spriteBatch, SpriteFont font)
        {
            _buttonManager.Draw(spriteBatch, font);
        }

        public Texture2D DrawTexture
        {
            get { return _textureCircle; }
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
            if (!_game.IsActive)
            {
                return;
            }
            MouseState m = Mouse.GetState();

            foreach (GameWindow g in _windowManager.Windows)
            {
                if (g.Visible)
                {
                    if (
                        new Rectangle(g.AbsoluteRect.X, g.AbsoluteRect.Y, g.AbsoluteRect.Width, g.AbsoluteRect.Height).Contains(
                            new Point(m.X, m.Y)))
                    {
                        return;
                    }
                }
            }

            foreach (Button b in _buttonManager.Buttons)
            {
                if (b.Area.Contains(new Point(m.X, m.Y)))
                {
                    return;
                }
            }

            switch (_controlMode)
            {
                case EControlMode.AddObstacle:
                    foreach (Box b in GetOverlappedBoxes())
                    {
                        if (b.Area.Intersects(MouseArea))
                        {
                            this.AddObstacleRobot(b.Position);
                            b.IsObstacle = true;
                        }
                    }
                    break;
                case EControlMode.AddPoints:
                    if (_points.Count == 0)
                    {
                        _points.Add(new DestinationPoint(this, _cam.ScreenToWorld(new Vector2(m.X, m.Y)), true));
                    }
                    else
                    {
                        _points.Add(new DestinationPoint(this, _cam.ScreenToWorld(new Vector2(m.X, m.Y)), _points.Count == 0 ? null : _points[_points.Count - 1]));
                    }

                    break;
            }


        }

        public void AddObstacleRobot(Microsoft.Xna.Framework.Vector2 position)
        {
            _robotControl.SendRequestRobot("AddObstacle=" + position.X + ";" + position.Y);
        }

        public void OnRightClick()
        {
            if (!_game.IsActive)
            {
                return;
            }
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
                Microsoft.Xna.Framework.Vector2 pos = _cam.ScreenToWorld(new Microsoft.Xna.Framework.Vector2(ms.X, ms.Y));
                return new Rectangle((int)pos.X, (int)pos.Y, 5, 5);
            }
        }

        public Box GetBoxAt(int x, int y)
        {
            if (x < 0 || y < 0 || x >= boxCountPerLine || y >= boxCountPerLine)
                return null;

            return _boxes[x, y];
        }

        public Box this[int x, int y]
        {
            get
            {
                if (x < 0 || y < 0 || x >= boxCountPerLine || y >= boxCountPerLine)
                    return null;

                return _boxes[x / BoxWidthCm, y / BoxWidthCm];
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
                    if (GetBoxAt(i, j) != null)
                    {
                        Box b = GetBoxAt(j, i);
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



        internal void OnExit(object sender, EventArgs e)
        {
            _exited = true;
        }
    }
}
