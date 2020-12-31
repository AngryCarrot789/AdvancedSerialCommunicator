using AdvancedSerialCommunicator.Messaging;
using AdvancedSerialCommunicator.Serial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRFramework.Utilities;

namespace AdvancedSerialCommunicator.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public SerialPortViewModel Port { get; set; }

        public MainViewModel()
        {
            Port = new SerialPortViewModel();
        }
    }
}
