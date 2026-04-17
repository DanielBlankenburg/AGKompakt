using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes.ActivityRequestClasses
{
    public class ActivityRequestComment
    {
        public int ActivityRequestCommentID { get; set; }
        public string Text { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public int ActivityRequestID { get; set; }
        public ActivityRequest ActivityRequest { get; set; }
    }
}
