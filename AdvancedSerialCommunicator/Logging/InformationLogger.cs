using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AdvancedSerialCommunicator.Logging
{
    public static class InformationLogger
    {
        public delegate void InfoLog(string text);
        public static event InfoLog OnLog;

        public static void Log(string info)
        {
            Application.Current?.Dispatcher?.Invoke(()=> { OnLog?.Invoke(info); });
        }
    }
}
