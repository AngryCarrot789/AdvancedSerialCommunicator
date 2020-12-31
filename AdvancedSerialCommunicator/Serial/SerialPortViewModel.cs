using AdvancedSerialCommunicator.Logging;
using AdvancedSerialCommunicator.Messaging;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRFramework.Utilities;

namespace AdvancedSerialCommunicator.Serial
{
    public class SerialPortViewModel : BaseViewModel
    {
        private bool _isConnected;
        private bool _isReceiveThreadActive;
        private string _connectedPort;

        public bool IsConnected
        {
            get => _isConnected;
            set => RaisePropertyChanged(ref _isConnected, value);
        }

        public bool IsReceiveThreadActive
        {
            get => _isReceiveThreadActive;
            set => RaisePropertyChanged(ref _isReceiveThreadActive, value);
        }

        public string ConnectedPort
        {
            get => _connectedPort;
            set => RaisePropertyChanged(ref _connectedPort, value);
        }

        public Command AutoConnectDisconnectCommand { get; }
        public Command ResetSerialPortCommand { get; }
        public Command AutoStartStopReceiverCommand { get; }
        public CommandParam<string> ClearBuffersCommand { get; }

        public PortSettingsViewModel Settings { get; set; }
        public MessageReceiver Receiver { get; set; }
        public MessageSender Sender { get; set; }
        public MessagesViewModel Messages { get; set; }
        public MessageBoxLogger Boxes { get; set; }

        private SerialPort Port { get; set; }

        public SerialPortViewModel()
        {
            Port = new SerialPort();
            Port.ErrorReceived += Port_ErrorReceived;

            Settings = new PortSettingsViewModel();
            Settings.SettingsChangedCallback = UpdateSettings;
            Settings.TimeoutsChangedCallback = UpdateTimeouts;
            Settings.BufferSizesChangedCallback = UpdateBufferSizes;
            Settings.ThingsChangedCallback = UpdateThings;

            Boxes = new MessageBoxLogger();

            Sender = new MessageSender();
            Sender.UpdateSerialPort(Port);
            Sender.Logger = Boxes;

            Messages = new MessagesViewModel();
            Messages.SendMessageCallback = Sender.SendMessageLine;
            Boxes.LogCallback = LogMessages;

            Receiver = new MessageReceiver();
            Receiver.MessageReceivedCallback = Messages.MessageReceived;
            Receiver.UnprocessedMessageCallback = Messages.UnprocessedBufferMessage;
            Receiver.UpdateSerialPort(Port);

            UpdateSettings();
            UpdateTimeouts();
            UpdateBufferSizes();
            UpdateThings();

            AutoConnectDisconnectCommand = new Command(AutoConnectDisconnect);
            ResetSerialPortCommand = new Command(ResetSerialPort);
            AutoStartStopReceiverCommand = new Command(AutoStartStopReceiver);
            ClearBuffersCommand = new CommandParam<string>(DiscardPortBuffers);

            StartMessageReceiver();
        }

        private void Port_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            switch (e.EventType)
            {
                case SerialError.Frame:
                    LogError($"Framing error detected: attempted to read from wrong starting point of data");
                    LogError("Solution: Reset SerialPort");
                    break;
                case SerialError.Overrun:
                    LogError($"Overrun error detected: data arrived before previous data could be processed");
                    LogError("Solution: Reset SerialPort");
                    break;
                case SerialError.RXOver:
                    LogError($"RXOver error detected: the receive buffer is full, or data was received after end-of-file marker");
                    LogError("Solution: Clear Receiver Buffers");
                    break;
                case SerialError.RXParity:
                    LogError($"RXParity error detected: parity might not have been applied, or data was corrupted");
                    LogError("Solution: Restart Application");
                    break;
                case SerialError.TXFull:
                    LogError($"TXFull error detected: attempted to transmit data when output buffer was full");
                    LogError("Solution: Clear Transmit Buffers");
                    break;
            }
        }

        private void LogError(string errormesage)
        {
            LogMessages($"Error: {errormesage}", true);
        }

        private void LogMessages(string text, bool writeToReceived)
        {
            if (writeToReceived)
            {
                Messages.AddReceivedMessage(text);
            }
            else
            {
                Messages.AddSentMessage(text);
            }
        }

        public void AutoConnectDisconnect()
        {
            IsConnected = Port.IsOpen;
            if (IsConnected)
            {
                Disconnect();
            }
            else
            {
                Connect();
            }
        }

        public void Connect()
        {
            if (IsConnected)
            {
                Messages.Log("Already connected");
                return;
            }

            if (Settings.GetCOMPort() == "COM1")
            {
                Messages.Log("Cannot connect to COM1 because it's a system thing");
                return;
            }

            ConnectedPort = Settings.GetCOMPort();
            Messages.WriteConnectInformation(Settings);

            try
            {
                Port.Open();
                IsConnected = true;
            }
            catch (Exception e)
            {
                Messages.WriteConnectionFailed(e, ConnectedPort);
                return;
            }

            Messages.WriteConnectionSuccess(ConnectedPort);
        }

        public void Disconnect()
        {
            if (!IsConnected)
            {
                Messages.Log("Already disconnected");
                return;
            }

            Messages.WriteDisconnectInformation(ConnectedPort);

            try
            {
                Port.Close();
                IsConnected = false;
            }
            catch (Exception e)
            {
                Messages.WriteConnectionFailed(e, ConnectedPort);
                return;
            }

            Receiver.StopReceiving();
            Messages.WriteDisconnectSuccess(ConnectedPort);
            ConnectedPort = "(None)";
        }

        public void AutoStartStopReceiver()
        {
            if (Receiver.IsEnabled)
            {
                Receiver.StopReceiving();
            }
            else
            {
                Receiver.StartReceiving();
            }

            IsReceiveThreadActive = Receiver.IsEnabled;
        }

        public void StartMessageReceiver()
        {
            try
            {
                Receiver.StartReceiving();
            }
            catch (Exception e)
            {
                Messages.Log($"Failed to start message receiver: {e.Message}");
            }

            IsReceiveThreadActive = Receiver.IsEnabled;
        }

        public void StopMessageReceiver()
        {
            try
            {
                Receiver.StopReceiving();
            }
            catch (Exception e)
            {
                Messages.Log($"Failed to stop message receiver: {e.Message}");
            }

            IsReceiveThreadActive = Receiver.IsEnabled;
        }

        public void DiscardPortBuffers(string buffer)
        {
            if (!IsConnected)
            {
                Messages.Log("You need to be connected to a port to discard the buffers");
            }
            else
            {
                switch (buffer)
                {
                    case "r": Port.DiscardInBuffer(); break;
                    case "w": Port.DiscardOutBuffer(); break;
                    case "a":
                        Port.DiscardInBuffer();
                        Port.DiscardOutBuffer();
                        break;
                }
            }
        }

        /// <summary>
        /// Use incase something bad happens that isn't recoverable
        /// </summary>
        public void ResetSerialPort()
        {
            if (IsConnected)
            {
                DiscardPortBuffers("a");
                Disconnect();
            }
            Port.Dispose();
            Port = null;
            Port = new SerialPort();
            Port.ErrorReceived += Port_ErrorReceived;
            Sender.UpdateSerialPort(Port);
            Receiver.UpdateSerialPort(Port);
            UpdateSettings();
            UpdateTimeouts();
            UpdateBufferSizes();
            UpdateThings();
            LogMessages($"Successfully reset SerialPort", true);
        }

        public void UpdateSettings()
        {
            if (IsConnected)
            {
                Messages.Log("Already connected to a port, cannot change settings. Disconnect first");
            }
            else
            {
                Port.PortName = Settings.GetCOMPort();
                Port.BaudRate = Settings.GetBaudRate();
                Port.DataBits = Settings.GetDataBits();
                Port.StopBits = Settings.GetStopBits();
                Port.Parity = Settings.GetParity();
                Port.Handshake = Settings.GetHandshake();
            }
        }

        public void UpdateTimeouts()
        {
            Port.ReadTimeout = Settings.GetReadTimeout();
            Port.WriteTimeout = Settings.GetWriteTimeout();
        }

        public void UpdateBufferSizes()
        {
            if (IsConnected)
            {
                Messages.Log("Already connected to a port, cannot change buffer sizes. Disconnect first");
            }
            else
            {
                Port.ReadBufferSize = Settings.GetReadBufferSize();
                Port.WriteBufferSize = Settings.GetWriteBufferSize();
            }
        }

        // Update "Things"
        public void UpdateThings()
        {
            Port.DtrEnable = Settings.GetDTR();
            Port.RtsEnable = Settings.GetRTS();
        }
    }
}
