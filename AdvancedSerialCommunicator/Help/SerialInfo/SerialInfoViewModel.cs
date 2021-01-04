using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AdvancedSerialCommunicator.Help.SerialInfo
{
    public class SerialInfoViewModel
    {
        public string Header { get; set; }

        public string Information { get; set; }

        public SerialInfoViewModel()
        {
            Header = "Something";
            Information = "Information about Something";
        }

        public SerialInfoViewModel(string header, string info)
        {
            Header = header;
            Information = info;
        }
    }
}
