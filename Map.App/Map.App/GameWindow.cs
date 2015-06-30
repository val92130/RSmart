using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TomShane.Neoforce.Controls;

namespace Map.App
{
    public class GameWindow : Window
    {
        public GameWindow(Manager manager) : base(manager)
        {
        }

        public GameWindow(Manager manager, string title, Rectangle position)
            : base(manager)
        {
            this.Text = title;
            this.Top = position.Y;
            this.Left = position.X;
            this.Width = position.Width;
            this.Height = position.Height;
            this.Init();
        }
    }
}
