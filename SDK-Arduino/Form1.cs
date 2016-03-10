using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace SDK_Arduino
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            getAvailablePorts();
        }

        void getAvailablePorts()
        {
            String[] ports = SerialPort.GetPortNames();
            comboBox1.Items.AddRange(ports);
        }

        //public delegate void InvokeDelegate();

        //public void text_Close()
        //{
        //    textBox2.Text = "Closeando\n";
        //    serialPort1.Close();
        //}
        //public void text_Open()
        //{
        //    textBox2.Text = "**** Connected to " + comboBox1.Text + " @ " + comboBox2.Text + " baud ****\n";
        //}

        //public void text_EnterKey()
        //{
        //    textBox2.Text = ">>>>" + textBox1.Text + "\n";
        //}
        //public void text_DataReceived(string datain)
        //{
        //    textBox2.Text = datain + "\n";
        //}

        private void button3_Click(object sender, EventArgs e) // Open conection
        {
            try
            {
                if (comboBox1.Text == "" || comboBox2.Text == "")
                    textBox2.Text = "Please select port settings\n";
                else
                {
                    serialPort1.PortName = comboBox1.Text;
                    serialPort1.BaudRate = Convert.ToInt32(comboBox2.Text);
                    serialPort1.Open();
                    if (serialPort1.IsOpen)
                    {
                        progressBar1.Value = 100;
                        button1.Enabled = true;
                        textBox1.Enabled = true;
                        button3.Enabled = false;
                        button4.Enabled = true;
                        //textBox2.BeginInvoke(new InvokeDelegate(text_Open));
                        textBox2.Invoke(new Action<string>(textBox2.AppendText), new object[] { "**** Connected to " + comboBox1.Text + " @ " + comboBox2.Text + " baud ****\n" });
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                textBox2.Text = "Unauthorized Access\n";
            }
        }

        private void button4_Click(object sender, EventArgs e) // Close Connection
        {
            serialPort1.Close();
            progressBar1.Value = 0;
            if (serialPort1.IsOpen == false)
            {
                button1.Enabled = false;
                textBox1.Enabled = false;
                button3.Enabled = true;
                button4.Enabled = false;
                //textBox2.BeginInvoke(new InvokeDelegate(text_Close));
                textBox2.Invoke(new Action<string>(textBox2.AppendText), new object[] { "**** Connection closed ****\n" });
            }
        }

        private void button1_Click(object sender, EventArgs e) // Send code with Send button
        {
            serialPort1.WriteLine(textBox1.Text);
            textBox1.Text = "";
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            //textBox2.BeginInvoke(new InvokeDelegate(text_DataReceived));
            textBox2.Invoke(new Action<string>(textBox2.AppendText), new object[] {indata});
        }

        private void enterkey(object sender, KeyEventArgs e) // Send code with Enter Key
        {
            if (e.KeyCode == Keys.Enter)
            {
                serialPort1.WriteLine(textBox1.Text);
                //textBox2.BeginInvoke(new InvokeDelegate(text_EnterKey));
                textBox2.Invoke(new Action<string>(textBox2.AppendText), new object[] { ">>>>" + textBox1.Text + "\n" });
                textBox1.Text = "";
            }
        }
    }
}
