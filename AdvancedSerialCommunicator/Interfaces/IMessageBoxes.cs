using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedSerialCommunicator.Interfaces
{
    public interface IMessageBoxes
    {
        void ScrollReceivedToBottom();
        void ScrollSentToBottom();
    }
}
