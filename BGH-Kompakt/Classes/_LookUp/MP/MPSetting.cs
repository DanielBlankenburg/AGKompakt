using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BGH_Kompakt.Enums.SettingEnums;

namespace BGH_Kompakt.Classes._LookUp.MP
{
    public class MPSetting
    {
        public int MPSettingID { get; set; }
        public string MPSettingEMailAnrede { get; set; }
        public string MPSettingEMailSchluss { get; set; }
        public string MPSettingDatenschutzhinweis { get; set; }
        public bool UploadBSCWServer { get; set; }
        public Drives? BSCWServerDrive { get; set; } = new Drives();



    }
}
