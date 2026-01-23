using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using ClosedXML.Excel;

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

            LoadSoftwareVersion();

        }

        private void LoadSoftwareVersion()
        {
            // Get the assembly version
            Version version = Assembly.GetExecutingAssembly().GetName().Version;

            // Set the Form Title to include the version
            this.Text = $"Serial Monitor - v{version}";
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
        StringBuilder _stringBuilder = new StringBuilder();
        public void DisplayDataReceived(string data)
        {
            _stringBuilder.Append(data);
            try
            {
                Invoke(new Action(() =>
                {
                    if (isTimeStamp)
                    {
                        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); // Format the timestamp
                        string timestampedData = $"{timestamp} -> {data} "; // Combine timestamp and data and add space at the end
                        richTextBoxOutput.AppendText(timestampedData + Environment.NewLine); // Append the new data with a new line
                    }
                    else
                    {
                        //richTextBoxOutput.AppendText(_stringBuilder + " "); // Append the new data with a space

                        // Append only the new data received to new line
                        richTextBoxOutput.AppendText(_stringBuilder + Environment.NewLine);
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

            if (_stringBuilder.Length > 44)
            {
                //WriteToFile(_stringBuilder.ToString());
                SaveToExcel(_stringBuilder.ToString());

                _stringBuilder.Clear();
            }
        }

        private bool isLoggingEnabled = false; // Initially disabled
        /// <summary>
        /// Writes the provided data string to a log file named "MessageLog.txt" in the application's directory.
        /// If the file does not exist, it creates it first.
        /// The data is appended as a new line to the end of the log file.
        /// Includes basic error handling to catch and display any exceptions that occur during file operations.
        /// This function is conditionally executed based on the 'isLoggingEnabled' flag;
        /// data is only written to the file if logging is currently enabled.
        /// </summary>
        /// <param name="data">The string of data to write to the log file.</param>
        private void WriteToFile(string data)
        {
            if (isLoggingEnabled) // Only write if logging is enabled
            {
                if (!File.Exists("./Serial Monitor Weld Results.txt"))
                {
                    FileStream fileStream = File.Create("./Serial Monitor Weld Results.txt");
                    fileStream.Close();
                }

                try
                {
                    using (StreamWriter streamWriter = File.AppendText("./Serial Monitor Weld Results.txt"))
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.Append(data);
                        streamWriter.WriteLine(stringBuilder);
                        streamWriter.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("WriteToFile", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        double number;

        private void SaveToExcel(string cleanedData)
        {
            if (!isLoggingEnabled) return;

            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WeldResults.xlsx");

            // Safety check: Don't crash if the file is open
            if (IsFileLocked(new FileInfo(filePath)))
            {
                richTextBoxOutput.Invoke(new Action(() =>
                    richTextBoxOutput.AppendText("\r\n[Error] Excel file is open! Close it to save data.\r\n")));
                return;
            }

            try
            {
                using (var workbook = File.Exists(filePath) ? new XLWorkbook(filePath) : new XLWorkbook())
                {
                    // Get or create the worksheet
                    var worksheet = workbook.Worksheets.Count > 0
                        ? workbook.Worksheet(1)
                        : workbook.Worksheets.Add("Weld Data");

                    // 1. SETUP HEADERS (Only runs if the sheet is empty)
                    if (worksheet.LastRowUsed() == null)
                    {
                        string[] headers = {
                    "Weld Energy (J)", "Max Power (W)", "Power (W)",
                    "Weld Time (ms)", "Power Loss (W)", "Frequency (Hz)",
                    "Status 1", "Status 2", "Weld Count"
                };

                        for (int i = 0; i < headers.Length; i++)
                        {
                            worksheet.Cell(1, i + 1).Value = headers[i];
                            worksheet.Cell(1, i + 1).Style.Font.Bold = true; // Make headers bold
                        }
                    }

                    // 2. PARSE THE DATA
                    // Split the "cleanedData" string wherever there is a comma
                    string[] values = cleanedData.Split(',');

                    // Find the next empty row
                    int nextRow = worksheet.LastRowUsed()?.RowNumber() + 1 ?? 1;

                    // 3. WRITE DATA TO CELLS
                    for (int i = 0; i < values.Length; i++)
                    {
                        // Declare the variable OUTSIDE the TryParse function
                        double number;

                        // Now simply use "out number" instead of "out double number"
                        if (double.TryParse(values[i], out number))
                        {
                            worksheet.Cell(nextRow, i + 1).Value = number;
                        }
                        else
                        {
                            worksheet.Cell(nextRow, i + 1).Value = values[i];
                        }
                    }

                    // Optional: Auto-fit columns to make it look nice
                    worksheet.Columns().AdjustToContents();

                    workbook.SaveAs(filePath);
                }
            }
            catch (Exception ex)
            {
                richTextBoxOutput.Invoke(new Action(() =>
                    richTextBoxOutput.AppendText($"\r\n[Error] Save failed: {ex.Message}\r\n")));
            }
        }

        // Keep the IsFileLocked helper from before
        private bool IsFileLocked(FileInfo file)
        {
            if (!file.Exists) return false;
            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                return true;
            }
            return false;
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

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (btnFind.BackColor == Color.WhiteSmoke)
            {
                gBFind.Visible = true;
                btnFind.BackColor = Color.Lime;
            }
            else
            {
                gBFind.Visible = false;
                btnFind.BackColor = Color.WhiteSmoke;
            }
        }

        private void btnEnableLogging_Click(object sender, EventArgs e)
        {
            isLoggingEnabled = !isLoggingEnabled; // Toggle the logging state

            if (isLoggingEnabled)
            {
                btnEnableLogging.Text = "Disable Logging";
                btnEnableLogging.BackColor = Color.LimeGreen; // Optional: Indicate enabled state
            }
            else
            {
                btnEnableLogging.Text = "Enable Logging";
                btnEnableLogging.BackColor = SystemColors.ControlLight; // Optional: Indicate disabled state
            }
        }
    }
}
