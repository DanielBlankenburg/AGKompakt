using BGH_Kompakt.Classes.ActivityRequestClasses;
using System.Collections.Generic;

namespace BGH_Kompakt.Classes._LookUp.ActivityRequestLookUps
{
    public class ActivityRequestScienceCategorie
    {
        public int ActivityRequestScienceCategorieId { get; set; }
        public string ActivityRequestScienceCategorieText { get; set; }
        public IList<ActivityRequest> ActivityRequests { get; set; }

    }
}
