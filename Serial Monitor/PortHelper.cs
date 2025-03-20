using System;
using System.Collections.Generic;
using System.IO.Ports;
using Microsoft.Win32;

namespace Serial_Monitor
{
    public class PortHelper
    {
        // Function to get the current COM and LPT ports
        public static List<string> GetAvailablePorts()
        {
            List<string> ports = new List<string>();

            // Get all COM ports
            string[] comPorts = SerialPort.GetPortNames();
            ports.AddRange(comPorts);

            // Get all LPT ports (from registry)
            using (RegistryKey lptKey = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DEVICEMAP\PARALLEL PORTS"))
            {
                if (lptKey != null)
                {
                    foreach (string portName in lptKey.GetValueNames())
                    {
                        ports.Add(portName);
                    }
                }
            }

            return ports;
        }

        public SerialPort serialPort;

        // Function to connect to the selected port
        public bool ConnectToPort(string portName, int baudRate = 9600)
        {
            try
            {
                // Initialize SerialPort with selected port name and default settings
                serialPort = new SerialPort
                {
                    PortName = portName,
                    BaudRate = baudRate,  // Set baud rate (default 9600)
                    Parity = Parity.None,
                    DataBits = 8,
                    StopBits = StopBits.One,
                    Handshake = Handshake.None
                };

                serialPort.DataReceived += SerialPort_DataReceived;

                // Open the port
                serialPort.Open();
                if (serialPort.IsOpen)
                {
                    Console.WriteLine($"Successfully connected to {portName}");                  
                    return true; // Connection successful
                }             
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to port: {ex.Message}");
            }

            return false; // Connection failed
        }

        // Function to disconnect from the port
        public void DisconnectFromPort()
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
                Console.WriteLine("Disconnected from the port.");
            }
        }

        public delegate void EventHandler(string data); // Delegate definition
        public event EventHandler SerialDataReceived; // Event declaration

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = serialPort.ReadExisting();

            if (SerialDataReceived != null) // Check if there are subscribers
            {
                SerialDataReceived(data);
            }
        }

        public bool UpdateConnectionStatus()
        {            
            return serialPort.IsOpen;
        }
    }
}
