using ServerLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VocalCommand
{
    public partial class Form1 : Form
    {
        RobotControl _robotControl;
        public Form1()
        {
            InitializeComponent();

            _robotControl = new RobotControl();

            SpeechRecognizer recognizer = new SpeechRecognizer();

            Choices directions = new Choices();
            directions.Add(new string[] { "for", "back", "left", "right", "go", "basta" });

            GrammarBuilder gb = new GrammarBuilder();
            gb.Append(directions);

            Grammar g = new Grammar(gb);
            g.Priority = 127;
            recognizer.LoadGrammar(g);

            recognizer.SpeechRecognized +=
              new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);
        }
        void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            switch(e.Result.Text)
            {
                case "for":
                    _robotControl.SendRequestRobot("Forward=true");
                    break;
                case "go":
                    _robotControl.SendRequestRobot("Start=true");
                    break;
                case "basta":
                    _robotControl.SendRequestRobot("Stop=true");
                    break;
                case "back":
                    _robotControl.SendRequestRobot("Backward=true");
                    break;
                case "left":
                    _robotControl.SendRequestRobot("Left=true");
                    break;
                case "right":
                    _robotControl.SendRequestRobot("Right=true");
                    break;
                
            }
        }
    }
}
