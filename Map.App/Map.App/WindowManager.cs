using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Microsoft.Xna.Framework;
using TomShane.Neoforce.Controls;
using EventArgs = TomShane.Neoforce.Controls.EventArgs;

namespace Map.App
{
    public class WindowManager
    {
        private Manager _manager;
        private List<GameWindow> _windows = new List<GameWindow>();
        private MainGame _mainGame;
        private TextBox _savePathTextBox;
        public WindowManager(MainGame mainGame, Game game, GraphicsDeviceManager graphics )
        {
            _mainGame = mainGame;
            _manager = new Manager(game,graphics);
        }

        public void Update(GameTime gametime)
        {
            _manager.Update(gametime);
        }

        public void ShowLoadPathWindow(Rectangle position)
        {
            string[] files = System.IO.Directory.GetFiles(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "*.path");

            GameWindow window = new GameWindow(_manager, "Load Path", position);

            window.Height = files.Length*40 + 100;

            int paddingY = 40;
            foreach (string s in files)
            {
                string name = Path.GetFileNameWithoutExtension(s);
                TomShane.Neoforce.Controls.Button button = new TomShane.Neoforce.Controls.Button(_manager);
                button.Init();
                button.Text = name;
                button.Width = 150;
                button.Height = 24;
                button.Left = (window.ClientWidth / 2) - (button.Width / 2);
                button.Top = paddingY;
                button.Anchor = Anchors.Bottom;
                button.Parent = window;
                button.Focused = true;
                button.Click += new TomShane.Neoforce.Controls.EventHandler(LoadButtonClick);
                paddingY += 40;
            }

            _windows.Add(window);
            window.Center();
            window.Visible = true;


            _manager.Add(window);
        }

        private void LoadButtonClick(object sender, EventArgs e)
        {
            TomShane.Neoforce.Controls.Button button = sender as TomShane.Neoforce.Controls.Button;
            string fileName = button.Text;

            if(!File.Exists((Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\" + fileName + ".path")))
            {
                return;
            }

            try
            {
                using (Stream stream = File.Open(fileName + ".path", FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();

                    var points = (List<DestinationPoint>)bin.Deserialize(stream);
                    foreach (DestinationPoint d in points)
                    {
                        d.SetMainGame(_mainGame);
                    }
                    _mainGame.LoadPoints(points);
                }
            }
            catch (IOException)
            {
            }

        }

        public void ShowSavePathWindow(Rectangle position)
        {
            GameWindow window = new GameWindow(_manager, "Save Path", position ); 
            _windows.Add(window);
            window.Center();
            window.Visible = true;

            TomShane.Neoforce.Controls.Button button = new TomShane.Neoforce.Controls.Button(_manager);
            button.Init();
            button.Text = "Save";
            button.Width = 72;
            button.Height = 24;
            button.Left = (window.ClientWidth / 2) - (button.Width / 2);
            button.Top = window.ClientHeight - button.Height - 8;
            button.Anchor = Anchors.Bottom;
            button.Parent = window;
            button.Focused = true;
            button.Click += new TomShane.Neoforce.Controls.EventHandler(SaveButtonClick);

            _savePathTextBox = new TextBox(_manager);
            _savePathTextBox.Init();
            _savePathTextBox.Parent = window;
            _savePathTextBox.Left = 32;
            _savePathTextBox.Top = 32;
            _savePathTextBox.Width = window.ClientWidth - 64;
            _savePathTextBox.TextChanged += new TomShane.Neoforce.Controls.EventHandler(TextChanged);

            _savePathTextBox.Text = "Choose a filename";
            _savePathTextBox.Anchor = Anchors.Left | Anchors.Top | Anchors.Right;

            _manager.Add(window);
        }

        private void TextChanged(object sender, System.EventArgs e)
        {
            TextBox t = sender as TextBox;
            t.ToolTip.Text = t.Text;
        }

        private void SaveButtonClick(object sender, System.EventArgs e)
        {
            if(_savePathTextBox != null)
                _mainGame.SavePath(_savePathTextBox.Text);
        }

        public void AddWindow(string title, Rectangle position)
        {
            GameWindow g = new GameWindow(_manager, title, position);
            _windows.Add(g);
            _manager.Add(g);
        }

        public IEnumerable<GameWindow> Windows
        {
            get { return _windows; }
        }

        public void Initialize()
        {
            _manager.SkinDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\..\..\..\Skins\";
            _manager.Initialize();
        }

        public void BeginDraw(GameTime gametime)
        {
            _manager.BeginDraw(gametime);
        }

        public void EndDraw(GameTime gametime)
        {
            _manager.EndDraw();
        }
    }
}
