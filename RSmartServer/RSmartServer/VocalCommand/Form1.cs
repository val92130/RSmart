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
            directions.Add(new string[] { "avant", "arriere", "gauche", "droite", "demarre", "stop" });

            GrammarBuilder gb = new GrammarBuilder();
            gb.Append(directions);

            Grammar g = new Grammar(gb);
            recognizer.LoadGrammar(g);

            recognizer.SpeechRecognized +=
              new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);
        }
        void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            switch(e.Result.Text)
            {
                case "avant":
                    _robotControl.SendRequestRobot("Forward=true");
                    break;
                case "demarre":
                    _robotControl.SendRequestRobot("Start=true");
                    break;
                case "stop":
                    _robotControl.SendRequestRobot("Stop=true");
                    break;
                case "arriere":
                    _robotControl.SendRequestRobot("Backward=true");
                    break;
                case "gauche":
                    _robotControl.SendRequestRobot("Left=true");
                    break;
                case "droite":
                    _robotControl.SendRequestRobot("Right=true");
                    break;
            }
        }
    }
}
