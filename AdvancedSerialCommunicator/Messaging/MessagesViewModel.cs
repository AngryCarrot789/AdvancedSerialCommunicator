using AdvancedSerialCommunicator.Interfaces;
using AdvancedSerialCommunicator.Serial;
using AdvancedSerialCommunicator.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRFramework.Utilities;

namespace AdvancedSerialCommunicator.Messaging
{
    public class MessagesViewModel : BaseViewModel
    {
        private string _receivedText;
        private string _sendText;
        private string _toBeSentMessage;
        private int _messagesReceived;
        private int _messagesSent;

        public string ReceivedText
        {
            get => _receivedText;
            set => RaisePropertyChanged(ref _receivedText, value);
        }

        public string SentText
        {
            get => _sendText;
            set => RaisePropertyChanged(ref _sendText, value);
        }

        public string ToBeSentMessage
        {
            get => _toBeSentMessage;
            set => RaisePropertyChanged(ref _toBeSentMessage, value);
        }

        public int MessagesReceived
        {
            get => _messagesReceived;
            set => RaisePropertyChanged(ref _messagesReceived, value);
        }

        public int MessagesSent
        {
            get => _messagesSent;
            set => RaisePropertyChanged(ref _messagesSent, value);
        }

        public Command ClearReceivedCommand { get; }
        public Command ClearSentCommand { get; }
        public Command SendMessageCommand { get; }

        public IMessageBoxes MessageBoxes { get; internal set; }

        public Action<string, ExecutableStatus> SendMessageCallback { get; internal set; }

        public MessagesViewModel()
        {
            SentText = "";
            ReceivedText = "";
            ToBeSentMessage = "";

            ClearReceivedCommand = new Command(ClearReceived);
            ClearSentCommand = new Command(ClearSent);
            SendMessageCommand = new Command(SendMessage);
        }

        public void Message(DateTime time, MessageDirection direction, string message)
        {
            string displayMessage = MessageFormatter.FormatMessage(time, direction, message);

            if (direction == MessageDirection.TX)
            {
                AddSentMessage(displayMessage);
            }
            else if (direction == MessageDirection.RX || direction == MessageDirection.Buffer)
            {
                AddReceivedMessage(displayMessage);
            }
        }

        public void MessageSent(DateTime time, string message)
        {
            Message(time, MessageDirection.TX, message);
        }

        public void MessageSent(string message)
        {
            Message(DateTime.Now, MessageDirection.TX, message);
        }

        public void MessageReceived(DateTime time, string message)
        {
            Message(time, MessageDirection.RX, message);
        }

        public void MessageReceived(string message)
        {
            Message(DateTime.Now, MessageDirection.RX, message);
        }

        public void UnprocessedBufferMessage(string message)
        {
            string msg = MessageFormatter.FormatDirection(MessageDirection.Buffer);
            AddReceivedMessage(msg);
        }

        public void AddReceivedMessage(string message)
        {
            // Stops the sent messages clogging up with trillions of stuff idk
            if (ShouldClearReceived())
            {
                ClearReceived();
            }

            ReceivedText += message + '\n';
            MessagesReceived++;
            MessageBoxes.ScrollReceivedToBottom();
        }

        public void AddSentMessage(string message)
        {
            // Stops the sent messages clogging up with trillions of stuff idk
            if (ShouldClearSent())
            {
                ClearSent();
            }

            SentText += message + '\n';
            MessagesSent++;
            MessageBoxes.ScrollSentToBottom();
        }

        public void ClearReceived()
        {
            ReceivedText = "";
            MessagesReceived = 0;
        }

        public void ClearSent()
        {
            SentText = "";
            MessagesSent = 0;
        }

        private void SendMessage()
        {
            if (!string.IsNullOrEmpty(ToBeSentMessage))
            {
                ExecutableStatus executable = new ExecutableStatus();
                SendMessageCallback?.Invoke(ToBeSentMessage, executable);
                if (executable.CanExecute)
                {
                    MessageSent(ToBeSentMessage);
                }
            }
        }

        public bool ShouldClearReceived()
        {
            return MessagesReceived > GlobalSettings.MAX_RECEIVED_MESSAGES || 
                ReceivedText.Length > GlobalSettings.MAX_RECEIVED_CHARACTER_COUNT;
        }

        public bool ShouldClearSent()
        {
            return MessagesSent > GlobalSettings.MAX_SENT_MESSAGES ||
                SentText.Length > GlobalSettings.MAX_SENT_CHARACTER_COUNT;
        }

        #region Serial Port Logging (Sort of)

        public void Log(string message)
        {
            AddReceivedMessage(message);
        }

        public void WriteConnectInformation(PortSettingsViewModel settings)
        {
            AddReceivedMessage($"Connecting to port: {settings.GetCOMPort()}");
            AddReceivedMessage($"- with baud rate: {settings.GetBaudRate()}");
            AddReceivedMessage($"- with data bits: {settings.GetDataBits()}");
            AddReceivedMessage($"- with stop bits: {settings.GetStopBits()}");
            AddReceivedMessage($"- with parity: {settings.GetParity()}");
            AddReceivedMessage($"- with handshake: {settings.GetHandshake()}");
            AddReceivedMessage($"- Timeouts -- Read: {settings.GetReadTimeout()}, Write: {settings.GetWriteTimeout()}");
            AddReceivedMessage($"- Buffer Sizes -- Read: {settings.GetReadBufferSize()}, Write: {settings.GetWriteBufferSize()}");
            AddReceivedMessage("Connecting now...");
        }

        public void WriteDisconnectInformation(string port)
        {
            AddReceivedMessage($"Attempting to disconnect from port: {port}");
            AddReceivedMessage("Disconnecting now...");
        }

        public void WriteConnectionSuccess(string port = null)
        {
            if (port == null)
            {
                AddReceivedMessage("Successfully connected!");
            }
            else
            {
                AddReceivedMessage($"Successfully connected to port: {port}!");
            }
        }

        public void WriteConnectionFailed(Exception exception, string port = null)
        {
            if (port == null)
            {
                AddReceivedMessage($"Failed to connect to port. Exception: {exception.Message}");
            }
            else
            {
                AddReceivedMessage($"Failed to connect to port: {port}. Exception: {exception.Message}");
            }
        }

        public void WriteDisconnectSuccess(string port = null)
        {
            if (port == null)
            {
                AddReceivedMessage("Successfully disconnected from port!");
            }
            else
            {
                AddReceivedMessage($"Successfully disconnected from port: {port}!");
            }
        }

        public void WriteDisconnectFailed(Exception exception, string port = null)
        {
            if (port == null)
            {
                AddReceivedMessage($"Failed to disconnect from port. Exception: {exception.Message}");
            }
            else
            {
                AddReceivedMessage($"Failed to disconnect from port: {port}. Exception: {exception.Message}");
            }
        }

        #endregion
    }
}
