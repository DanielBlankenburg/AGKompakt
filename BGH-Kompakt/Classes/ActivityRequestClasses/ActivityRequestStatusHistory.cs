using BGH_Kompakt.Classes._LookUp.ActivityRequestLookUps;
using BGH_Kompakt.Classes.ActivityRequestClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes.ActivityRequestClasses
{
    public class ActivityRequestStatusHistory
    {
        public int ActivityRequestStatusHistoryID { get; set; }
        public int ActivityRequestStatusID { get; set; }
        public virtual ActivityRequestStatus ActivityRequestStatus { get; set; }
        public DateTime Date { get; set; }
        public int ActivityRequestID { get; set; }
        public ActivityRequest ActivityRequest { get; set; }
    }
}
