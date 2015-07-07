using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Newtonsoft.Json;
using Server.Lib;
using TomShane.Neoforce.Controls;
using EventArgs = System.EventArgs;
using EventHandler = System.EventHandler;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Map.App
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D texture;
        private MouseState _mouse, lastMouseState, currentMouseState;
        private MainGame _mainGame;

        private SpriteFont _font;

        KeyboardState oldState;

        public bool left, right, up, down, zoomUp, zoomDown = false;

        public event EventHandler OnClicked;
        private DebugLog _debugLog = new DebugLog();
        private WebServer _server;
        public Game1()
        {
            Thread serverThread = new Thread(() =>
            {
                _server = new WebServer("http://" + Util.GetIp() + ":80/", _debugLog);
                _server.Run();
            });
            serverThread.Start();
            
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1366;
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferMultiSampling = true;
            IsFixedTimeStep = false;
            
            _mainGame = new MainGame(10000, this, 1366, 768);


            _server.OnRobotLostEvent += WebServerOnOnRobotLostEvent;
            this.Exiting += new EventHandler<EventArgs>(_mainGame.OnExit);
        }

        private void WebServerOnOnRobotLostEvent(object sender)
        {
            _mainGame.OnRobotLost();
        }

        protected override void Initialize()
        {
            
            base.Initialize();
            _mainGame.Initialize(this, graphics);
        }

        protected override void LoadContent()
        {
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            _mainGame.LoadContent(Content, GraphicsDevice);

            _font = Content.Load<SpriteFont>("SpriteFont1");
            texture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            texture.SetData<Color>(new Color[] { Color.White });

        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public void Clicked()
        {
            if (OnClicked != null)
            {
                OnClicked(this, null);
            }
        }
        protected override void Update(GameTime gameTime)
        {
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            base.Update(gameTime);

            HandleInput();
            _mainGame.Update(gameTime);
            if (up)
            {
                _mainGame.Camera.Pos = Vector2.Lerp(_mainGame.Camera.Pos, new Vector2(_mainGame.Camera.Pos.X, _mainGame.Camera.Pos.Y - 100), 0.05f);
            }
            if (left)
            {
                _mainGame.Camera.Pos = Vector2.Lerp(_mainGame.Camera.Pos, new Vector2(_mainGame.Camera.Pos.X - 100, _mainGame.Camera.Pos.Y), 0.05f);
            }
            if (right)
            {
                _mainGame.Camera.Pos = Vector2.Lerp(_mainGame.Camera.Pos, new Vector2(_mainGame.Camera.Pos.X + 100, _mainGame.Camera.Pos.Y), 0.05f);
            }
            if (down)
            {
                _mainGame.Camera.Pos = Vector2.Lerp(_mainGame.Camera.Pos, new Vector2(_mainGame.Camera.Pos.X, _mainGame.Camera.Pos.Y + 100), 0.05f);
            }


            if (zoomDown)
            {
                _mainGame.Camera.Zoom -= 0.1f;
            }

            if (zoomUp)
            {
                _mainGame.Camera.Zoom += 0.1f;
            }


        }


        public void HandleInput()
        {
            if (!this.IsActive)
            {
                return;
            }

            _mouse = Mouse.GetState();

            MouseState m = Mouse.GetState();

            lastMouseState = currentMouseState;


            currentMouseState = Mouse.GetState();

            if (lastMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
            {
                Rectangle area = new Rectangle(this.Window.ClientBounds.X, this.Window.ClientBounds.Y, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);
                if (!area.Contains(new Point(m.X + this.Window.ClientBounds.X, m.Y + this.Window.ClientBounds.Y)))
                    return;
                Clicked();
                _mainGame.OnLeftClick();
            }


            if (_mouse.RightButton == ButtonState.Pressed)
            {
                _mainGame.OnRightClick();
            }

            
            KeyboardState newState = Keyboard.GetState();

            // Zoom

            if (newState.IsKeyDown(Keys.PageDown))
            {
                if (!oldState.IsKeyDown(Keys.PageDown))
                {
                    zoomDown = true;
                }
            }
            else if (oldState.IsKeyDown(Keys.PageDown))
            {
                zoomDown = false;
            }

            // Zoom up

            if (newState.IsKeyDown(Keys.PageUp))
            {
                if (!oldState.IsKeyDown(Keys.PageUp))
                {
                    zoomUp = true;
                }
            }
            else if (oldState.IsKeyDown(Keys.PageUp))
            {
                zoomUp = false;
            }


            // Up
            if (newState.IsKeyDown(Keys.Up))
            {
                if (!oldState.IsKeyDown(Keys.Up))
                {
                    up = true;
                }
            }
            else if (oldState.IsKeyDown(Keys.Up))
            {
                up = false;
            }

            // down 

            if (newState.IsKeyDown(Keys.Down))
            {
                if (!oldState.IsKeyDown(Keys.Down))
                {
                    down = true;
                }
            }
            else if (oldState.IsKeyDown(Keys.Down))
            {
                down = false;
            }

            // left 

            if (newState.IsKeyDown(Keys.Left))
            {
                if (!oldState.IsKeyDown(Keys.Left))
                {
                    left = true;
                }
            }
            else if (oldState.IsKeyDown(Keys.Left))
            {
                left = false;
            }

            // right

            if (newState.IsKeyDown(Keys.Right))
            {
                if (!oldState.IsKeyDown(Keys.Right))
                {
                    right = true;
                }
            }
            else if (oldState.IsKeyDown(Keys.Right))
            {
                right = false;
            }

            oldState = newState;
        }

        protected override void Draw(GameTime gameTime)
        {
            
            _mainGame.WindowManager.BeginDraw(gameTime);
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Immediate,
                        BlendState.AlphaBlend,
                        null,
                        null,
                        null,
                        null,
                        _mainGame.Camera.GetTransformation());

            
            

            _mainGame.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);          
    
            spriteBatch.Begin();          
            _mainGame.DrawGUI(spriteBatch, _font);
            spriteBatch.Draw(texture, new Vector2(Mouse.GetState().X, Mouse.GetState().Y), _mainGame.MouseArea, Color.White);
            spriteBatch.End();
            _mainGame.WindowManager.EndDraw(gameTime);

            
        }
    }
}
