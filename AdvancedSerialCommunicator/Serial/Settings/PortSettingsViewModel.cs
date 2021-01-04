using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRFramework.Utilities;

namespace AdvancedSerialCommunicator.Serial.Settings
{
    public class PortSettingsViewModel : BaseViewModel
    {
        private bool _canEditControls;
        private string _comPort;
        private string _baudRate;
        private string _dataBits;
        private SettingsItemViewModel _stopBits;
        private string _parity;
        private SettingsItemViewModel _handShake;
        private double _readTimeout;
        private double _writeTimeout;
        private double _readBufferSize;
        private double _writeBufferSize;
        private bool _dataTerminalReady;
        private bool _requestToTransmit;
        private bool _breakState;
        private bool _discardNullCharacters;

        public ObservableCollection<string> AvaliableCOMPorts { get; set; }

        public Command RefreshCOMPortsCommand { get; }

        public Action SettingsChangedCallback { get; internal set; }
        public Action TimeoutsChangedCallback { get; internal set; }
        public Action BufferSizesChangedCallback { get; internal set; }
        public Action SoftwareHardwareSettingsChangedCallback { get; internal set; }
        public Action BreakStateChangedCallback { get; internal set; }

        public bool CanEditControls
        {
            get => _canEditControls;
            set => RaisePropertyChanged(ref _canEditControls, value);
        }

        public string SelectedCOMPort
        {
            get => _comPort;
            set => RaisePropertyChanged(ref _comPort, value, SettingsChangedCallback);
        }

        public string SelectedBaudRate
        {
            get => _baudRate;
            set => RaisePropertyChanged(ref _baudRate, value, SettingsChangedCallback);
        }

        public string SelectedDataBits
        {
            get => _dataBits;
            set => RaisePropertyChanged(ref _dataBits, value, SettingsChangedCallback);
        }

        public SettingsItemViewModel SelectedStopBits
        {
            get => _stopBits;
            set => RaisePropertyChanged(ref _stopBits, value, SettingsChangedCallback);
        }

        public string SelectedParity
        {
            get => _parity;
            set => RaisePropertyChanged(ref _parity, value, SettingsChangedCallback);
        }

        public SettingsItemViewModel SelectedHandshake
        {
            get => _handShake;
            set => RaisePropertyChanged(ref _handShake, value, SettingsChangedCallback);
        }

        public double ReadTimeout
        {
            get => _readTimeout;
            set => RaisePropertyChanged(ref _readTimeout, value, TimeoutsChangedCallback);
        }

        public double WriteTimeout
        {
            get => _writeTimeout;
            set => RaisePropertyChanged(ref _writeTimeout, value, TimeoutsChangedCallback);
        }

        public double ReadBufferSize
        {
            get => _readBufferSize;
            set => RaisePropertyChanged(ref _readBufferSize, value, BufferSizesChangedCallback);
        }

        public double WriteBufferSize
        {
            get => _writeBufferSize;
            set => RaisePropertyChanged(ref _writeBufferSize, value, BufferSizesChangedCallback);
        }

        public bool DataTerminalReady
        {
            get => _dataTerminalReady;
            set => RaisePropertyChanged(ref _dataTerminalReady, value, SoftwareHardwareSettingsChangedCallback);
        }

        public bool RequestToTransmit
        {
            get => _requestToTransmit;
            set => RaisePropertyChanged(ref _requestToTransmit, value, SoftwareHardwareSettingsChangedCallback);
        }

        public bool BreakState
        {
            get => _breakState;
            set => RaisePropertyChanged(ref _breakState, value, BreakStateChangedCallback);
        }

        public bool DiscardNullCharacters
        {
            get => _discardNullCharacters;
            set => RaisePropertyChanged(ref _discardNullCharacters, value, SoftwareHardwareSettingsChangedCallback);
        }

        public PortSettingsViewModel()
        {
            RefreshCOMPortsCommand = new Command(RefreshCOMPorts);

            AvaliableCOMPorts = new ObservableCollection<string>();
            RefreshCOMPorts();

            SelectedBaudRate = "115200";
            SelectedDataBits = "8";
            SelectedParity = "None";

            ReadTimeout = 500;
            WriteTimeout = 500;
            ReadBufferSize = 4096;
            WriteBufferSize = 4096;
            DataTerminalReady = false;
            RequestToTransmit = false;
        }

        public void RefreshCOMPorts()
        {
            AvaliableCOMPorts.Clear();
            foreach (string comPort in SerialPort.GetPortNames())
            {
                AvaliableCOMPorts.Add(comPort);
            }

            if (AvaliableCOMPorts.Count > 0)
            {
                SelectedCOMPort = AvaliableCOMPorts[0];
            }
            else
            {
                // default to COM1 just in case
                SelectedCOMPort = "COM1";
            }
        }

        public string GetCOMPort()
        {
                // default to COM1 just in case
            return SelectedCOMPort ?? "COM1";
        }

        public int GetBaudRate()
        {
            if (int.TryParse(SelectedBaudRate ?? "0", out int baudRate))
            {
                return baudRate;
            }
            return 0;
        }

        public int GetDataBits()
        {
            if (int.TryParse(SelectedDataBits ?? "8", out int dataBits))
            {
                return dataBits;
            }
            return 0;
        }
        
        public StopBits GetStopBits()
        {
            if (SelectedStopBits != null)
            {
                if (Enum.TryParse(SelectedStopBits.RealName, out StopBits stopBits))
                {
                    return stopBits;
                }
            }
            return StopBits.One;
        }

        public Parity GetParity()
        {
            if (Enum.TryParse(SelectedParity ?? "None", out Parity parity))
            {
                return parity;
            }
            return Parity.None;
        }

        public Handshake GetHandshake()
        {
            if (SelectedHandshake != null)
            {
                if (Enum.TryParse(SelectedHandshake.RealName, out Handshake handshake))
                {
                    return handshake;
                }
            }
            return Handshake.None;
        }

        public int GetReadTimeout()
        {
            return Convert.ToInt32(ReadTimeout);
        }

        public int GetWriteTimeout()
        {
            return Convert.ToInt32(WriteTimeout);
        }

        public int GetReadBufferSize()
        {
            return Convert.ToInt32(ReadBufferSize);
        }

        public int GetWriteBufferSize()
        {
            return Convert.ToInt32(WriteBufferSize);
        }

        public bool GetDTR()
        {
            return DataTerminalReady;
        }

        public bool GetRTS()
        {
            return RequestToTransmit;
        }

        public bool GetBreakState()
        {
            return BreakState;
        }

        public bool GetDiscardNull()
        {
            return DiscardNullCharacters;
        }
    }
}
