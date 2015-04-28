using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserInterface
{
    public partial class RouteCreationForm : Form
    {
        WebServer _server;
        public RouteCreationForm(WebServer server)
        {
            InitializeComponent();
            _server = server;
        }

        private void buttonAddRoute_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(textBoxKey.Text) || !string.IsNullOrWhiteSpace(textBoxValue.Text) || !string.IsNullOrWhiteSpace(richTextBoxResponse.Text))
            {
                _server.AddRoute(textBoxKey.Text, textBoxValue.Text, richTextBoxResponse.Text);
                textBoxKey.Text = "";
                textBoxValue.Text = "";
                richTextBoxResponse.Text = "";
            }
        }
    }
}
