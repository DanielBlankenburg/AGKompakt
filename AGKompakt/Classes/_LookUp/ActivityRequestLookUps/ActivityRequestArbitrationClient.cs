using BGH_Kompakt.Classes.ActivityRequestClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes._LookUp.ActivityRequestLookUps
{
    public class ActivityRequestArbitrationClient
    {
        public int ActivityRequestArbitrationClientId { get; set; }
        public string ActivityRequestArbitrationClientText { get; set; }
        public ActivityRequest ActivityRequest { get; set; }

    }
}
