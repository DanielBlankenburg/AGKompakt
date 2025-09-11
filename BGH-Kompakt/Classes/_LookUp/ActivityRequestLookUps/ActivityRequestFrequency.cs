using BGH_Kompakt.Classes.ActivityRequestClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes._LookUp.ActivityRequestLookUps
{
    public class ActivityRequestFrequency
    {
        public int ActivityRequestFrequencyId { get; set; }
        public string ActivityRequestFrequencyText { get; set; }
        public IList<ActivityRequest> ActivityRequests { get; set; }

    }
}
