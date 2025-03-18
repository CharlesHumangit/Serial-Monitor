using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Serial_Monitor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            List<string> ports = PortHelper.GetAvailablePorts();
            foreach (string port in ports)
            {
                // Add ports to a dropdown (ComboBox) or display them
                cBPortName.Items.Add(port);
            }

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            cBPortName.Items.Clear();
            cBPortName.Text = "";

            List<string> ports = PortHelper.GetAvailablePorts();
            foreach (string port in ports)
            {
                // Add ports to a dropdown (ComboBox) or display them
                cBPortName.Items.Add(port);
            }
        }

        PortHelper portHelper = new PortHelper();
        string selectedPort;
        int baudRate;
        private void btnConnect_Click(object sender, EventArgs e)
        {                    
            if (cBBaudRate.SelectedItem != null && cBPortName.SelectedItem !=null)
            {
                selectedPort = cBPortName.SelectedItem.ToString();
                baudRate = int.Parse(cBBaudRate.SelectedItem.ToString());

                if (btnConnect.Text == "Connect")
                {
                    if (portHelper.ConnectToPort(selectedPort, baudRate))
                    {
                        MessageBox.Show($"Connected to {selectedPort}!");
                        btnConnect.Text = "Connected";
                        btnConnect.ForeColor = Color.Green;
                        btnConnect.Font = new Font(btnConnect.Font, FontStyle.Bold);
                        portHelper.SerialDataReceived += DisplayDataReceived;
                    }
                    else
                    {
                        MessageBox.Show($"Failed to connect to {selectedPort}.");
                    }
                }
                else
                {
                    portHelper.DisconnectFromPort();
                    MessageBox.Show("Disconnected from the port.");
                    btnConnect.Text = "Connect";
                    btnConnect.ForeColor = Color.Black;
                    btnConnect.Font = new Font(btnConnect.Font, FontStyle.Regular);
                }
            }
            else
            {
                MessageBox.Show("A field is empty.");
            }
        }

        bool isAutoscroll;
        public void DisplayDataReceived(string data)
        {
            Invoke(new Action(() =>
            {
                richTextBoxOutput.AppendText(data + Environment.NewLine); // Append the new data

                if (isAutoscroll)
                {
                    richTextBoxOutput.ScrollToCaret(); // Ensure the latest text is visible
                }
                
            }));
        }

        private void btnAutoscroll_Click(object sender, EventArgs e)
        {
            if (btnAutoscroll.BackColor == Color.WhiteSmoke)
            {
                isAutoscroll = true;
                btnAutoscroll.BackColor = Color.Lime;
            }
            else
            {
                isAutoscroll = false;
                btnAutoscroll.BackColor = Color.WhiteSmoke;
            }
        }

        private void btnClearTextBox_Click(object sender, EventArgs e)
        {
            richTextBoxOutput.Clear();
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            PortHelper portHelper = new PortHelper();
            portHelper.DisconnectFromPort();
        }
    }
}
