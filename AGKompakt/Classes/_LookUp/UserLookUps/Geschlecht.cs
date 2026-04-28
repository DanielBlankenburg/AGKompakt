using BGH_Kompakt.Classes.Base;
using BGH_Kompakt.Classes.UserClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes._LookUp.UserLookUps
{
    public class Geschlecht
    {
        public int GeschlechtID { get; set; }
        public string GeschlechtText { get; set; }
        public IList<User> Users { get; set; }
        public IList<Verfahrensbeistand> Verfahrensbeistaende { get; set; }

    }
}
