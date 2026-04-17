using BGH_Kompakt.Classes.ActivityRequestClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes._LookUp.ActivityRequestLookUps
{
    public class ARVerguetungAdventage
    {
        public int ARVerguetungAdventageId { get; set; }
        public int ARVerguetungAdventageTypId { get; set; }
        public virtual ARVerguetungAdventageTyp ARVerguetungAdventageTyp { get; set; }
        public decimal ARVerguetungAdventageAmount { get; set; }
        public ActivityRequest ActivityRequest { get; set; }

    }
}
