using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedSerialCommunicator.Utilities
{
    public static class GlobalSettings
    {
        public const int MAX_RECEIVED_MESSAGES = 500;
        public const int MAX_SENT_MESSAGES = 500;

        public const int MAX_RECEIVED_CHARACTER_COUNT = 25000;
        public const int MAX_SENT_CHARACTER_COUNT = 25000;
    }
}
