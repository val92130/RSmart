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
        private Texture2D _textureLine;
        private double _circleRadius;
        private MainGame _game;
        private bool _isRobotChild = false;
        public DestinationPoint(MainGame game, Vector2 position)
            : this(game, position, null)
        {
        }

        public DestinationPoint(MainGame game, Vector2 position, bool IsRobotChild)
            : this(game, position)
        {
            _isRobotChild = true;
            _circleRadius = Server.Lib.Vector2.Radius( new Server.Lib.Vector2( _position.X, _position.Y ),
                new Server.Lib.Vector2( _game.Robot.Orientation.X, _game.Robot.Orientation.Y ), new Server.Lib.Vector2( _game.Robot.Position.X + 1, _game.Robot.Position.Y + 1 ) );

            //_textureCircle = _game.CreateCircle( (int)_circleRadius );
        }

        public DestinationPoint(MainGame game, Vector2 position, DestinationPoint child)
        {
            _game = game;
            _position = position;
            _child = child;

            if (child != null)
            {
                
                _circleRadius = Server.Lib.Vector2.Radius( new Server.Lib.Vector2( _position.X, _position.Y ),
                new Server.Lib.Vector2( _game.Robot.Orientation.X, _game.Robot.Orientation.Y ), new Server.Lib.Vector2( child.Position.X, child.Position.Y ) );

                //_textureCircle = _game.CreateCircle((int)_circleRadius);
            }          
             
        }

        public Vector2 Position
        {
            get
            {
                return _position;
            }
        }


        public void Update()
        {
            
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D pointTexture, Texture2D circleTexture)
        {
            spriteBatch.Draw( pointTexture, this.Position, new Rectangle( (int)this.Position.X + 3, (int)this.Position.Y + 3, 6, 6 ), Color.White );

            if (_child != null)
            {
                MainGame.DrawLine( spriteBatch, this.Position, _child.Position, Color.Red, pointTexture );
                //spriteBatch.Draw( _textureCircle, new Vector2( -(_textureCircle.Width) + this.Position.X, _child.Position.Y  ), Color.Red );

            }

            if( _isRobotChild )
            {
                MainGame.DrawLine( spriteBatch, this.Position, _game.Robot.Position, Color.Red, pointTexture );
                //spriteBatch.Draw( _textureCircle, new Vector2( -(_textureCircle.Width) + this.Position.X, _game.Robot.Position.Y  ), Color.Red );

            }
        }

    }
}
