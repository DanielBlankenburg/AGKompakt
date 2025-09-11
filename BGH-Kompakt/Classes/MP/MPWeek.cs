using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes.MP
{
    public class MPWeek
    {
        public int MPWeekID { get; set; }
        public int MPWeekYear { get; set; }
        public int MPWeekNumber { get; set; }
        public string MPWeekName
        {
            get { return MPWeekNumber + ". KW"; }
        }

        public ICollection<MPDecision> MPDecisions { get; set; } = new List<MPDecision>();
    }
}
