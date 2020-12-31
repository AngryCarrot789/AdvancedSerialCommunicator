using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedSerialCommunicator.Messaging
{
    public enum MessageDirection
    {
        /// <summary>
        /// Received
        /// </summary>
        RX, 
        /// <summary>
        /// Sent/Transmitted
        /// </summary>
        TX,
        /// <summary>
        /// Fetched from the buffer after opening a serial port
        /// </summary>
        Buffer
    }
}
