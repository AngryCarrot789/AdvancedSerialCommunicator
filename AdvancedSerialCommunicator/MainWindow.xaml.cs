using AdvancedSerialCommunicator.Interfaces;
using AdvancedSerialCommunicator.ViewModels;
using System.Windows;

namespace AdvancedSerialCommunicator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMessageBoxes
    {
        public MainViewModel Model
        {
            get => this.DataContext as MainViewModel;
            set => this.DataContext = value;
        }

        public MainWindow()
        {
            InitializeComponent();
            // sort of dependency injecting... ish
            Model.Port.Messages.MessageBoxes = this;
        }

        public void ScrollReceivedToBottom()
        {
            Application.Current?.Dispatcher?.Invoke(() =>
            {
                receivedBox.ScrollToEnd();
            });
        }

        public void ScrollSentToBottom()
        {
            Application.Current?.Dispatcher?.Invoke(() =>
            {
                sentBox.ScrollToEnd();
            });
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Model.Port.StopMessageReceiver();
            Application.Current.Shutdown();
        }
    }
}
