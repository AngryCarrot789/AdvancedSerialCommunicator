using AdvancedSerialCommunicator.Help;
using AdvancedSerialCommunicator.Serial;
using TheRFramework.Utilities;

namespace AdvancedSerialCommunicator.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public SerialPortViewModel Port { get; set; }

        public HelpViewModel Help { get; set; }

        public MainViewModel()
        {
            Port = new SerialPortViewModel();
            Help = new HelpViewModel();
        }
    }
}
