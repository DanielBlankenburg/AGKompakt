using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes.ActivityRequestClasses
{
    public class ActivityRequestChangeHistory
    {
        public int ActivityRequestChangeHistoryID { get; set; }
        public string ActivityRequestChangeHistoryText { get; set; }
        public string ActivityRequestChangeHistoryAuthor { get; set; }
        public DateTime ActivityRequestChangeHistoryDate { get; set; }
        public int ActivityRequestId { get; set; }
        public ActivityRequest ActivityRequest { get; set; }

    }
}
