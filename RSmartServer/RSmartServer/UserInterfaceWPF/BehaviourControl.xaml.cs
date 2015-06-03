using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Server.Lib;

namespace Server.App
{
    /// <summary>
    /// Interaction logic for BehaviourControl.xaml
    /// </summary>
    public partial class BehaviourControl : Window
    {
        string behaviours = @"";
        string methods = @"";
        Dictionary<ComboBox, String> _values = new Dictionary<ComboBox, string>();
        private RobotControl _robotControl;
        public BehaviourControl(RobotControl robotControl)
        {

            _robotControl = robotControl;
            GetDataRobot();
            InitializeComponent();
            Initialize();
            
        }

        public void GetDataRobot()
        {
            if (!_robotControl.PingRobot())
            {
                MessageBox.Show("Robot seems to be offline");
                return;
            }               
            behaviours = _robotControl.SendRequestRobot("GetBehaviours=true");
            methods = _robotControl.SendRequestRobot("GetMethods=true");
        }

        public void SendDataRobot()
        {
            foreach (KeyValuePair<ComboBox, string> entry in _values)
            {
                _robotControl.SendRequestRobot(entry.Value + "=" + entry.Key.SelectedValue.ToString());
            }
        }

        public List<String> JsonToList(string json)
        {
            if (json == null)
                return null;
            string s;
            s = json;
            s = s.Replace(@"""", "");
            s = s.Replace("{", "");
            s = s.Replace("}", "");
            s = s.Replace(":", "-");

            return s.Split('-').ToList();
        }

        private void Initialize()
        {

            foreach (string str in JsonToList(behaviours))
            {
                Label l = new Label()
                {
                    Content = str + " : "
                };

                ComboBox c = new ComboBox()
                {
                    ItemsSource = JsonToList(methods),
                    SelectedIndex = 0
                };   

                _values.Add(c, str);

                stackPanel.Children.Add(l);
                stackPanel.Children.Add(c);
                TextBox t = new TextBox();
            }
            
            Button btn = new Button()
            {
                Name = "Validate",
                Content = "Validate",
                Margin = new Thickness(50,15,50,50),
                Width = 100,
                Height = 50,
                
            };
            btn.Click += new RoutedEventHandler(Button_validate);
            stackPanel.Children.Add(btn);
        }

        private void Button_validate(object sender, RoutedEventArgs e)
        {
            string str = "";
            if (_values.Count != 0)
            {
                foreach (KeyValuePair<ComboBox, string> entry in _values)
                {
                    str += entry.Value + " : " + entry.Key.SelectedValue.ToString() + "\n";
                }
            }
            MessageBox.Show( str );
            SendDataRobot();
            
        }
    }
}
