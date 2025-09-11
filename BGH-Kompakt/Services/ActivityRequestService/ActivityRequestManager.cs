using BGH_Kompakt.Classes.ActivityRequestClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Services.ActivityRequestService
{
    public static class ActivityRequestManager
    {
        public static ActivityRequest SelectedActivityRequest { get; set; } = new ActivityRequest();
        //public static int BereichsArt { get; set; } // 1 = Normale List // 2 = Adminliste // 3 = NewRequest // 4 = RequestOverview
        //public static bool EditStatus { get; set; } = false;
        public static int LoginType { get; set; } = 0; //1 = User //2 = ARAdmin, Präsidialrichter, Präsidentin, Vorsitzender Richter
        public static int ActionType { get; set; } = 0; //1 = Create //2 = Update // 3 = Duplicate
        public static int ListArt { get; set; } = 0; // 1 = Overview // 2 = OverviewOpenRequests  // 3 = ArchivRequests// 
        public static int AblageArt { get; set; } = 0; // 1 = Antragsbereich ; 2 = Präsidialrichterbereich; 3 = Präsidentenbereich; 4 = Vorzimmer; 5 = Archiv; 6 = Vorsitzender Richter
        public static bool DirectJump { get; set; } = false;

    }
}
