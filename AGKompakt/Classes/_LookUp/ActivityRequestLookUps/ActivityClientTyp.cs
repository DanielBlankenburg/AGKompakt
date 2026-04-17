using BGH_Kompakt.Classes.ActivityRequestClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes._LookUp.ActivityRequestLookUps
{
    public class ActivityClientTyp
    {
        public int ActivityClientTypId { get; set; }
        public string ActivityClientTypText { get; set; }
        public IList<ActivityClient> activityClients { get; set; }
    }
}
