using AdvancedSerialCommunicator.Logging;
using AdvancedSerialCommunicator.Utilities;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedSerialCommunicator.Messaging
{
    /// <summary>
    /// Used for sending messages on the same thread. pretty much useless but still... organised
    /// </summary>
    public class MessageSender
    {
        public bool CanSend { get; set; }

        private SerialPort Port { get; set; }

        public MessageBoxLogger Logger { get; set; }

        public MessageSender()
        {
            CanSend = true;
        }

        public void SendMessageLine(string message, ExecutableStatus successful)
        {
            MessagingError error = SendMessage(message, true);
            if (error == MessagingError.TimeoutException)
            {
                Logger?.LogSent("Timed out when trying to send. Possibly because the recipient wasn't connected to the other port");
            }
            successful.CanExecute = error == MessagingError.None;
        }

        public MessagingError SendMessage(string message, bool newLine = true)
        {
            try
            {
                string newMessage = message + (newLine ? "\n" : "");
                byte[] buffer = Port.Encoding.GetBytes(newMessage);
                for(int i = 0; i < buffer.Length; i++)
                {
                    Port.BaseStream.WriteByte(buffer[i]);
                }
                return MessagingError.None;
            }
            catch(TimeoutException timeout)
            {
                InformationLogger.Log($"Timeout Exception. {timeout.Message}");
                return MessagingError.TimeoutException;
            }
            catch(Exception e)
            {
                InformationLogger.Log(e.StackTrace);
                return MessagingError.GeneralError;
            }
        }

        public void UpdateSerialPort(SerialPort port)
        {
            Port = port;
        }
    }
}
