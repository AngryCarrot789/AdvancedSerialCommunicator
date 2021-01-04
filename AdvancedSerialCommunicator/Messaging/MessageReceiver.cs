using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdvancedSerialCommunicator.Messaging
{
    /// <summary>
    /// A class used for receiving messages on another thread
    /// </summary>
    public class MessageReceiver
    {
        private Thread ReceiverThread { get; set; }

        public bool IsEnabled { get; set; }

        private bool ShouldFullyShutdownThread { get; set; }

        private SerialPort Port { get; set; }

        public Action<string> MessageReceivedCallback { get; internal set; }
        public Action<string> BufferDataFoundCallback { get; internal set; }

        public MessageReceiver()
        {
            IsEnabled = true;
            ReceiverThread = new Thread(ReceiveLoop);
            ReceiverThread.Start();
        }

        private void ReceiveLoop()
        {
            string message = "";
            char read;

            while (true)
            {
                if (ShouldFullyShutdownThread)
                {
                    return;
                }

                if (IsEnabled)
                {
                    if (Port == null)
                    {
                        Thread.Sleep(10);
                    }
                    else
                    {
                        if (Port.IsOpen)
                        {
                            while (Port.BytesToRead > 0)
                            {
                                if (IsEnabled)
                                {
                                    read = (char)Port.ReadChar();
                                    switch (read)
                                    {
                                        case '\r':
                                            break;
                                        case '\n':
                                            MessageReceivedCallback?.Invoke(message);
                                            message = "";
                                            break;
                                        default:
                                            message += read;
                                            break;
                                    }
                                }
                                else
                                {
                                    //if (!string.IsNullOrEmpty(message))
                                    //{
                                    //    UnprocessedMessageCallback?.Invoke(message);
                                    //    message = "";
                                    //}
                                    Thread.Sleep(10);
                                }
                            }
                        }
                        Thread.Sleep(10);
                    }
                }
                else
                {
                    Thread.Sleep(10);
                }
            }
        }

        public void ProcessBufferedString()
        {
            string message = "";
            char read;
            if (Port.IsOpen)
            {
                while (Port.BytesToRead > 0)
                {
                    read = (char)Port.ReadChar();
                    switch (read)
                    {
                        case '\r':
                            break;
                        case '\n':
                            BufferDataFoundCallback?.Invoke(message);
                            message = "";
                            break;
                        default:
                            message += read;
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(message))
                {
                    BufferDataFoundCallback?.Invoke(message);
                }
            }
        }

        public void StopReceiving()
        {
            IsEnabled = false;
        }

        public void StartReceiving()
        {
            IsEnabled = true;
        }

        public void UpdateSerialPort(SerialPort port)
        {
            StopReceiving();
            Port = port;
            StartReceiving();
        }

        /// <summary>
        /// Fully kills the thread. Only use when shutting down the app
        /// </summary>
        public void KillThreadLoop()
        {
            IsEnabled = false;
            ShouldFullyShutdownThread = true;
            ReceiverThread.Abort();
        }
    }
}
