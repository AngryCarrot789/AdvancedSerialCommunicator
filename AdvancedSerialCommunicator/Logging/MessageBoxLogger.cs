using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedSerialCommunicator.Logging
{
    /// <summary>
    /// A simple thing used for easily logging in the received messages box
    /// </summary>
    public class MessageBoxLogger
    {
        public Action<string, bool> LogCallback { get; internal set; }

        public void LogReceived(string str)
        {
            LogCallback?.Invoke(str, true);
        }

        public void LogSent(string str)
        {
            LogCallback?.Invoke(str, false);
        }
    }
}
