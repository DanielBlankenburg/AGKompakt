using BGH_Kompakt.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace BGH_Kompakt.Classes
{
    public class SystemSetting : ViewModelBase
    {
        private string _Laufwerk_BSCW_Server = string.Empty;

        public string Laufwerk_BSCW_Server
        {
            get => _Laufwerk_BSCW_Server.ToUpper();
            set => SetProperty<string>(ref _Laufwerk_BSCW_Server, value, nameof(_Laufwerk_BSCW_Server));
        }


        public SystemSetting() { }
        public SystemSetting(string Laufwerk_BSCW_Server)
        {
            _Laufwerk_BSCW_Server = Laufwerk_BSCW_Server;
        }

    }
}
