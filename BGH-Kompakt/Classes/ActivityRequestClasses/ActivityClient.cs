using BGH_Kompakt.Classes._LookUp.ActivityRequestLookUps;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BGH_Kompakt.Classes.ActivityRequestClasses
{
    public class ActivityClient
    {
        public int ActivityClientId { get; set; }
        public string ACName{ get; set; }
        public int ActivityRequestId { get; set; }
        public IList<ActivityRequest> ActivityRequests { get; set; }   
        public int ActivityClientTypID { get; set; }
        public virtual ActivityClientTyp ActivityClientTyp  { get; set; }

        [NotMapped]
        public string Category
        {
            get
            {
                return ActivityClientTyp.ActivityClientTypText ?? string.Empty;
            }
        }

    }
}
