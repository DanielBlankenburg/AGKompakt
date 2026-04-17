using BGH_Kompakt.Classes.MP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes._LookUp.MP
{
    public class MPCategory
    {
        public int MPCategoryID { get; set; }
        public string MPCategoryText { get; set; }
        public ICollection<MPSenat> MPSenate { get; set; } = new List<MPSenat>();
    }
}
