using BGH_Kompakt.Classes.UserClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes._LookUp.UserLookUps
{
    public class Sprache
    {
        public int SpracheID { get; set; }
        public string SpracheText { get; set; }
        public IList<Verfahrensbeistand> Verfahrensbeistaende { get; set; }
    }
}
