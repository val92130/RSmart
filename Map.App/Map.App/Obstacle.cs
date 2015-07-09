using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Map.App
{
    public class Obstacle
    {
        private Rectangle _area;
        private MainGame _game;
        public Obstacle(int x, int y, int width, int height, MainGame game)
        {
            _area = new Rectangle(x,y,width,height);
            _game = game;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_game.GetTexture(this), _area, Color.White);
        }
    }
}
