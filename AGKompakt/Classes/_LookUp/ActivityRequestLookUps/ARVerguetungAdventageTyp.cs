using BGH_Kompakt.Classes.ActivityRequestClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes._LookUp.ActivityRequestLookUps
{
    public class ARVerguetungAdventageTyp
    {
        public int ARVerguetungAdventageTypId { get; set; }
        public string ARVerguetungAdventageTypText { get; set; }
        public IList<ARVerguetungAdventage> ARVerguetungAdventages { get; set; }

    }
}
