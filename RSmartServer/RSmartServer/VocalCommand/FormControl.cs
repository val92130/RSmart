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
using System.Speech.Synthesis;

namespace VocalCommand
{
    public partial class audioControlForm : Form
    {
        RobotControl _robotControl;
        SpeechSynthesizer parole;
        Timer timerLoop;
        DebugLog _debugLog;
        public audioControlForm()
        {
            InitializeComponent();

            timerLoop = new Timer();
            timerLoop.Interval = 10;
            timerLoop.Tick += new EventHandler( T_loop );
            timerLoop.Start();


            _debugLog = new DebugLog();
            parole = new SpeechSynthesizer();
            _robotControl = new RobotControl();
            Say( "Hi " + Environment.UserName + ", welcome to the RSmart audio control interface" );

            if( Util.PingRobot() )
            {
                Say( "I'm online" );
            }
            else
            {
                Say( "I'm offline" );
            }


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

        private void T_loop( object sender, EventArgs e )
        {
            if(_debugLog.Count > 0)
            {
                textOutput.Text += _debugLog.Get + "\n";
                
            }
            
        }
        void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            switch(e.Result.Text)
            {
                case "for":
                    _robotControl.SendRequestRobot("Forward=true");
                    Say( RandomResponse() );
                    break;
                case "go":
                    _robotControl.SendRequestRobot("Start=true");
                    Say( RandomResponse() );
                    break;
                case "basta":
                    _robotControl.SendRequestRobot("Stop=true");
                    Say( RandomResponse() );
                    break;
                case "back":
                    _robotControl.SendRequestRobot("Backward=true");
                    Say( RandomResponse() );
                    break;
                case "left":
                    _robotControl.SendRequestRobot("Left=true");
                    break;
                case "right":
                    _robotControl.SendRequestRobot("Right=true");
                    Say( RandomResponse() );
                    break;
                default :
                    _debugLog.Write( "Exception : default reached" );
                    break;
            }
        }

        Random r = new Random();
        public string RandomResponse()
        {
            String[] responses = new String[]
            {
                "Good",
                "Perfect",
                "Okay",
                "No problem",
                "I'm on my way",
                "Sure !"
            };
            
            string resp = responses[r.Next( 0, responses.Length )];
            
            return resp;
        }

        public void Say(String text)
        {
            _debugLog.Write( "Robot said : " + text );
            parole.SpeakAsync( text );
        }
    }
}
