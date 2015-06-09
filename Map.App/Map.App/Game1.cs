using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

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

        private bool clicked = false;

        private MouseState _mouse;

        private MainGame _mainGame;

        private SpriteFont _font;

        KeyboardState oldState;

        public bool left, right, up, down, zoomUp, zoomDown = false;

        public event EventHandler OnClicked;
        public Game1()
        {
            
            _mainGame = new MainGame(10000, this);
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferMultiSampling = true;
            
        }

        protected override void Initialize()
        {
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            _mainGame.LoadContent(Content);

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
            _mouse = Mouse.GetState();

            if (_mouse.LeftButton == ButtonState.Pressed)
            {
                Clicked();
                _mainGame.OnLeftClick();
                clicked = true;
            }

            if (_mouse.LeftButton == ButtonState.Released)
            {
                clicked = false;
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
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Immediate,
                        BlendState.AlphaBlend,
                        null,
                        null,
                        null,
                        null,
                        _mainGame.Camera.GetTransformation());

            
            base.Draw(gameTime);
            _mainGame.Draw(spriteBatch);
            spriteBatch.Draw(texture, new Vector2(_mainGame.MouseArea.X, _mainGame.MouseArea.Y), _mainGame.MouseArea, Color.White );
            spriteBatch.End();
        }
    }
}
