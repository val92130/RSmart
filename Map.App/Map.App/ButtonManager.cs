using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Map.App
{
    public class ButtonManager
    {
        private MainGame _game;
        private List<Button> _buttons;
        public ButtonManager(MainGame game)
        {
            _game = game;
            _buttons = new List<Button>();
        }

        public void Add(Vector2 position, string text, int width, int height, Color color, MainGame game,
            MainGame.ClickHandler specifiClickHandler, MainGame.ClickHandler globalHandler)
        {
            this.Add(new Button(position, text, width, height, color, game, specifiClickHandler), globalHandler);
        }

        public void Add(Button b, MainGame.ClickHandler globalClickHandler)
        {
            _buttons.Add(b);
            b.OnClick += new Button.ClickEvent(ButtonClick);
            b.OnClick += new Button.ClickEvent(globalClickHandler);
        }

        private void ButtonClick(Button sender)
        {
            foreach (Button b in _buttons)
            {
                b.Selected = b == sender;
            }
            Debug.Print(sender.Area.ToString());
        }

        public List<Button> Buttons
        {
            get { return _buttons; }
        }

        public void LoadContent(ContentManager content, GraphicsDevice graphics)
        {
            foreach (Button b in _buttons)
            {
                b.LoadContent(graphics);
            }
        }

        public void Update()
        {
            foreach (Button b in _buttons)
            {
                b.Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            foreach (Button b in _buttons)
            {
                b.Draw(spriteBatch, font);
            }
        }
    }
}
