using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static SpeechRecognitionEngine recognitionEngine;
        static void Main( string[] arg )
        {
            recognitionEngine = new SpeechRecognitionEngine();
            recognitionEngine.SetInputToDefaultAudioDevice();
            recognitionEngine.SpeechRecognized += ( s, args ) =>
            {
                foreach( RecognizedWordUnit word in args.Result.Words )
                {
                    if( word.Confidence > 0.8f )
                        Console.WriteLine (word.Text + " ");
                }
            };
            recognitionEngine.LoadGrammar( new DictationGrammar() );
        }
    }
}
