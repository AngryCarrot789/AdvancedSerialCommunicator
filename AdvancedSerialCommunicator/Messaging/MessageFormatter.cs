using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedSerialCommunicator.Messaging
{
    public static class MessageFormatter
    {
        public static string FormatDirection(MessageDirection direction)
        {
            switch (direction)
            {
                case MessageDirection.RX:     return "[RX]>";
                case MessageDirection.TX:     return "[TX]>";
                case MessageDirection.Buffer: return "[Buffer]>";
                default:                      return "[Error]>";
            }
        }

        public static string FormatTime(DateTime time)
        {
            return $"[{time.ToString("T")}]";
        }

        public static string FormatMessage(DateTime time, MessageDirection direction, string message)
        {
            return $"{FormatTime(time)} {FormatDirection(direction)} {message}";
        }

        public static string FormatMessage(MessageDirection direction, string message)
        {
            return $"{FormatDirection(direction)} {message}";
        }

        public static string FormatBuffer(string message)
        {
            return $"[Buffered Data]> {message}";
        }
    }
}
