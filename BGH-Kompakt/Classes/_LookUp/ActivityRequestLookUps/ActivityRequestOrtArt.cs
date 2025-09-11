using BGH_Kompakt.Classes.ActivityRequestClasses;
using System.Collections.Generic;

namespace BGH_Kompakt.Classes._LookUp.ActivityRequestLookUps
{
    //[Table("ActivityRequestOrtArten")]
    public class ActivityRequestOrtArt
    {
        public int ActivityRequestOrtArtId { get; set; }
        public string ActivityRequestOrtArtText { get;set; }
        public IList<ActivityRequest> ActivityRequests { get; set; }

    }
}
