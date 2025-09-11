using BGH_Kompakt.Classes.MP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes._LookUp.MP
{
    public class MPBE
    {
        public int MPBEID { get; set; }
        public string MPBEName { get; set; }
        public ICollection<MPDecision> MPDecisions { get; set; } = new List<MPDecision>();

    }
}
