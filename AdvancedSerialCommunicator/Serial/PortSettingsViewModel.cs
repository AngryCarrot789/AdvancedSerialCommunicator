using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRFramework.Utilities;

namespace AdvancedSerialCommunicator.Serial
{
    public class PortSettingsViewModel : BaseViewModel
    {
        private string _comPort;
        private string _baudRate;
        private string _dataBits;
        private string _stopBits;
        private string _parity;
        private string _handShake;
        private double _readTimeout;
        private double _writeTimeout;
        private double _readBufferSize;
        private double _writeBufferSize;
        private bool _dataTerminalReady;
        private bool _requestToTransmit;

        public ObservableCollection<string> AvaliableCOMPorts { get; set; }

        public Command RefreshCOMPortsCommand { get; }

        public Action SettingsChangedCallback { get; internal set; }
        public Action TimeoutsChangedCallback { get; internal set; }
        public Action BufferSizesChangedCallback { get; internal set; }
        public Action ThingsChangedCallback { get; internal set; }

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

        public string SelectedStopBits
        {
            get => _stopBits;
            set => RaisePropertyChanged(ref _stopBits, value, SettingsChangedCallback);
        }

        public string SelectedParity
        {
            get => _parity;
            set => RaisePropertyChanged(ref _parity, value, SettingsChangedCallback);
        }

        public string SelectedHandshake
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
            set => RaisePropertyChanged(ref _dataTerminalReady, value, ThingsChangedCallback);
        }

        public bool RequestToTransmit
        {
            get => _requestToTransmit;
            set => RaisePropertyChanged(ref _requestToTransmit, value, ThingsChangedCallback);
        }

        public PortSettingsViewModel()
        {
            RefreshCOMPortsCommand = new Command(RefreshCOMPorts);

            AvaliableCOMPorts = new ObservableCollection<string>();
            RefreshCOMPorts();

            SelectedBaudRate = "115200";
            SelectedDataBits = "8";
            SelectedStopBits = "One";
            SelectedParity = "None";
            SelectedHandshake = "None";

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
            if (int.TryParse(SelectedBaudRate, out int baudRate))
            {
                return baudRate;
            }
            return 0;
        }

        public int GetDataBits()
        {
            if (int.TryParse(SelectedDataBits, out int dataBits))
            {
                return dataBits;
            }
            return 0;
        }
        
        public StopBits GetStopBits()
        {
            if (Enum.TryParse(SelectedStopBits, out StopBits stopBits))
            {
                return stopBits;
            }
            return StopBits.None;
        }

        public Parity GetParity()
        {
            if (Enum.TryParse(SelectedParity, out Parity parity))
            {
                return parity;
            }
            return Parity.None;
        }

        public Handshake GetHandshake()
        {
            if (Enum.TryParse(SelectedHandshake, out Handshake handshake))
            {
                return handshake;
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
    }
}
