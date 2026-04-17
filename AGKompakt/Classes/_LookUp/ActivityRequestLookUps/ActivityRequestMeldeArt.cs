using BGH_Kompakt.Classes.ActivityRequestClasses;
using System.Collections.Generic;

namespace BGH_Kompakt.Classes._LookUp.ActivityRequestLookUps
{
    public class ActivityRequestMeldeArt
    {
        public int ActivityRequestMeldeArtId { get; set; }
        public string ActivityRequestMeldeArtText { get; set; }
        public IList<ActivityRequest> ActivityRequests { get; set; }

    }
}
