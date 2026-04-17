using BGH_Kompakt.Classes.ActivityRequestClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes._LookUp.ActivityRequestLookUps
{
    public class ActivityRequestTyp
    {
        public int ActivityRequestTypId { get; set; }
        public string ActivityRequestTypText { get; set; }
        public int ActivityRequestTypMeldeArt {  get; set; }
        public IList<ActivityRequest> ActivityRequests { get; set; }

    }
}
