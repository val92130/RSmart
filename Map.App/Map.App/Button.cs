using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Map.App
{
    public class Button
    {
        private Vector2 _position;
        private MainGame _game;
        private Rectangle _area;
        private Texture2D texture;
        private Color _color;
        public delegate void ClickEvent(object sender);

        private MouseState lastMouseState, currentMouseState;

        public event ClickEvent OnClick;

        private MainGame.ClickHandler _handler;

        private string _text;
 
        public Button(Vector2 position, string text, int width, int height, Color color, MainGame game, MainGame.ClickHandler handler)
        {
            _text = text;
            _position = position;
            _game = game;
            _area = new Rectangle((int)_position.X, (int)_position.Y, width, height);
            _color = color;
            _handler = handler;
        }

        public void Click()
        {
            if (OnClick != null)
            {
                OnClick(this);  
            }

            if( _handler != null )
            {
                _handler( this );
            }
        }

        public void Update()
        {
            MouseState m = Mouse.GetState();

            lastMouseState = currentMouseState;


            currentMouseState = Mouse.GetState();

            if( lastMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed )
            {
                if (_area.Contains(new Point(m.X, m.Y)))
                {
                    Click();
                }
                
            }


        }

        public Rectangle Area
        {
            get { return _area; }
        }

        public void LoadContent(GraphicsDevice graphics)
        {
            texture = new Texture2D( graphics, 1, 1, false, SurfaceFormat.Color );
            texture.SetData<Color>(new Color[] { _color });
        }

        public void Draw( SpriteBatch spriteBatch, SpriteFont font )
        {
            
            Vector2 FontOrigin = font.MeasureString( _text ) / 2;
            spriteBatch.Draw( texture, _area, Color.White );
            int paddingWidth = (int)(_area.Width - FontOrigin.X)/2;
            int paddingHeight = (int)(_area.Height - FontOrigin.Y)/2;
            spriteBatch.DrawString( font, _text, _position + new Vector2( paddingWidth / 2, paddingHeight / 2 ), Color.Black );



        }

        


    }
}
