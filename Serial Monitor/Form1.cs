using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
                baudRate = cBBaudRate.SelectedIndex;

                if (btnConnect.Text == "Connect")
                {
                    if (portHelper.ConnectToPort(selectedPort, baudRate))
                    {
                        MessageBox.Show($"Connected to {selectedPort}!");
                        btnConnect.Text = "Connected";
                        btnConnect.ForeColor = Color.Green;
                        btnConnect.Font = new Font(btnConnect.Font, FontStyle.Bold);
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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            PortHelper portHelper = new PortHelper();
            portHelper.DisconnectFromPort();
        }
    }
}
