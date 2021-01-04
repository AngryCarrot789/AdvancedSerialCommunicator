using TheRFramework.Utilities;

namespace AdvancedSerialCommunicator.Help
{
    public class HelpViewModel
    {
        public Command AboutCommand { get; }
        public Command SerialInfoCommand { get; }

        private AboutWindow About { get; }
        private SerialInfoWindow SerialInfo { get; }

        public HelpViewModel()
        {
            About = new AboutWindow();
            SerialInfo = new SerialInfoWindow();
            AboutCommand = new Command(ShowHelpWindow);
            SerialInfoCommand = new Command(ShowSerialInfoWindow);
        }

        private void ShowSerialInfoWindow()
        {
            SerialInfo.Show();
        }

        private void ShowHelpWindow()
        {
            About.Show();
        }
    }
}
