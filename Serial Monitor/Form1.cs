using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Serial_Monitor
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// If styles like colors and fonts are reused elsewhere in your application, 
        /// consider centralizing these into constants or a resource file to enhance maintainability.
        /// </summary>
        private readonly Color ConnectedColor = Color.LimeGreen;
        private readonly Color DisconnectedColor = Color.LightCoral;
        private readonly FontStyle ConnectedFontStyle = FontStyle.Bold;
        private readonly FontStyle DisconnectedFontStyle = FontStyle.Regular;

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
            if (cBBaudRate.SelectedItem != null && cBPortName.SelectedItem != null)
            {
                selectedPort = cBPortName.SelectedItem.ToString();
                baudRate = int.Parse(cBBaudRate.SelectedItem.ToString());

                if (btnConnect.Text == "Connect")
                {
                    if (portHelper.ConnectToPort(selectedPort, baudRate))
                    {
                        MessageBox.Show($"Connected to {selectedPort}!");
                        btnConnect.Text = "Connected";
                        btnConnect.BackColor = ConnectedColor;
                        btnConnect.Font = new Font(btnConnect.Font, ConnectedFontStyle);
                        portHelper.SerialDataReceived += DisplayDataReceived;
                        timer1.Enabled = true;
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
                    btnConnect.BackColor = Color.WhiteSmoke;
                    btnConnect.Font = new Font(btnConnect.Font, ConnectedFontStyle);
                }
            }
            else
            {
                MessageBox.Show("A field is empty.");
            }
        }

        bool isAutoscroll;
        bool isTimeStamp;
        public void DisplayDataReceived(string data)
        {
            try
            {
                Invoke(new Action(() =>
                {
                    if (isTimeStamp)
                    {
                        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); // Format the timestamp
                        string timestampedData = $"{timestamp} -> {data}"; // Combine timestamp and data
                        richTextBoxOutput.AppendText(timestampedData); // Append the new data
                    }
                    else
                    {
                        richTextBoxOutput.AppendText(data); // Append the new data
                    }

                    if (isAutoscroll)
                    {
                        richTextBoxOutput.ScrollToCaret(); // Ensure the latest text is visible
                    }
                }));
            }
            catch (Exception ex)
            {
                this.Invoke(new Action(() => richTextBoxOutput.AppendText("Error reading data: " + ex.Message)));
            }
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
            portHelper.SerialDataReceived -= DisplayDataReceived;
            timer1.Enabled = false;
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            portHelper.DisconnectFromPort();
            MessageBox.Show("Disconnected from the port.");
            btnConnect.Text = "Connect";
            btnConnect.BackColor = Color.WhiteSmoke;
            btnConnect.Font = new Font(btnConnect.Font, FontStyle.Regular);
        }

        /// <summary>
        /// Instead of relying on a bool value for the connection state, 
        /// you can define an enum to make the code more readable and extensible.
        /// </summary>
        public enum ConnectionState
        {
            Connected,
            Disconnected
        }

        /// <summary>
        /// You are calling portHelper.UpdateConnectionStatus() multiple times within the same method. 
        /// Since the result does not change during execution, you can store it in a local variable to 
        /// enhance performance and readability.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            ConnectionState state = portHelper.UpdateConnectionStatus() ? ConnectionState.Connected : ConnectionState.Disconnected;
            UpdateConnectionStatusUI(state);
        }

        /// <summary>
        /// Extract the logic that updates the UI elements into a separate method. 
        /// This improves code modularity and reusability
        /// </summary>
        /// <param name="state"></param>
        private void UpdateConnectionStatusUI(ConnectionState state)
        {
            lblStatus.Text = state == ConnectionState.Connected ? $"Connected to {selectedPort}" : "No connection";
            lblStatus.BackColor = state == ConnectionState.Connected ? ConnectedColor : DisconnectedColor;
            btnConnect.Text = state == ConnectionState.Connected ? "Connected" : "Connect";
            btnConnect.Font = new Font(btnConnect.Font, state == ConnectionState.Connected ? ConnectedFontStyle : DisconnectedFontStyle);
            btnConnect.BackColor = state == ConnectionState.Connected ? ConnectedColor : Color.WhiteSmoke;
        }

        private void btnTimeStamp_Click(object sender, EventArgs e)
        {
            if (btnTimeStamp.BackColor == Color.WhiteSmoke)
            {
                isTimeStamp = true;
                btnTimeStamp.BackColor = Color.Lime;
            }
            else
            {
                isTimeStamp = false;
                btnTimeStamp.BackColor = Color.WhiteSmoke;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text;
            if (string.IsNullOrEmpty(searchText))
            {
                return; // Nothing to search for
            }

            lstSearchResults.Items.Clear(); // Clear previous results
            ClearHighlighting();

            int startIndex = 0;
            richTextBoxOutput.SelectionStart = 0;

            while (startIndex < richTextBoxOutput.TextLength)
            {
                int foundIndex = richTextBoxOutput.Find(searchText, startIndex, RichTextBoxFinds.None);

                if (foundIndex >= 0)
                {
                    lstSearchResults.Items.Add(foundIndex); // Add the found index to the list
                                                            //lstSearchResults.Items.Add(richTextBoxOutput.Text.Substring(Math.Max(0, foundIndex - 20), 
                                                            //Math.Min(40, richTextBoxOutput.TextLength - foundIndex)));
                    richTextBoxOutput.SelectionStart = foundIndex;
                    richTextBoxOutput.SelectionLength = searchText.Length;
                    richTextBoxOutput.SelectionBackColor = Color.Yellow;
                    startIndex = foundIndex + searchText.Length;
                }
                else
                {
                    break; // No more occurrences found
                }
            }
        }

        private void ClearHighlighting()
        {
            richTextBoxOutput.SelectionStart = 0;
            richTextBoxOutput.SelectionLength = richTextBoxOutput.TextLength;
            richTextBoxOutput.SelectionBackColor = richTextBoxOutput.BackColor; // Reset to default background
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ClearHighlighting();
        }

        private void lstSearchResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstSearchResults.SelectedIndex >= 0)
            {
                int selectedIndex = (int)lstSearchResults.SelectedItem;
                richTextBoxOutput.SelectionStart = selectedIndex;
                richTextBoxOutput.SelectionLength = txtSearch.Text.Length;
                richTextBoxOutput.ScrollToCaret(); // Scroll to the selected text
                richTextBoxOutput.Focus(); // Give focus to the textbox.
            }
        }
    }
}
