using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRFramework.Utilities;

namespace AdvancedSerialCommunicator.Serial.Settings
{
    public class SettingsItemViewModel : BaseViewModel
    {
        private string _previewName;
        private string _realName;

        public string PreviewName
        {
            get => _previewName;
            set => RaisePropertyChanged(ref _previewName, value);
        }

        public string RealName
        {
            get => _realName;
            set => RaisePropertyChanged(ref _realName, value);
        }

        public SettingsItemViewModel()
        {
            PreviewName = "Preview";
            RealName = "Real";
        }

        public SettingsItemViewModel(string preview, string realname)
        {
            PreviewName = preview;
            RealName = realname;
        }
    }
}
