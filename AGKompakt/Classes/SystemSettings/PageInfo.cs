using BGH_Kompakt.Classes.UserClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes.SystemSettings
{
    public class PageInfo
    {
        public string TopPage { get; set; }
        public string UnderPage { get; set; }
        public int UserSettingType { get; set; } = 0; //0 = User // 1 = Admin
        public User SelectedUser { get; set; }
    }
}
