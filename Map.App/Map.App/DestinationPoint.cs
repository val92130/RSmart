using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Map.App
{
    public class DestinationPoint
    {
        private Vector2 _position;
        private DestinationPoint _child;
        private Texture2D _texturePoint;
        private Texture2D _textureCircle;
        public DestinationPoint(Vector2 position)
        {
            _position = position;
            _child = null;
        }

        public DestinationPoint(Vector2 position, DestinationPoint child)
        {
            _position = position;
        }

        public void LoadContent(ContentManager content, GraphicsDevice graphics)
        {
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
        }
    }
}
