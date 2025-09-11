using BGH_Kompakt.Classes.MP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable

namespace BGH_Kompakt.Classes._LookUp.MP
{
    public class MPSenat
    {
        public int MPSenatID { get; set; }
        public string MPSenatName { get; set; }
        public int MPCategorieID { get; set; }
        public int MPSenatSorting { get; set; }
        public MPCategory MPCategory { get; set; }
        public int? MPDecisionID { get; set; }
        public ICollection<MPDecision> MPDecisions { get; set; } = new List<MPDecision>();

        public ICollection<MPSenatAbbreviation> MPSenatAbbreviations { get; set; } = new List<MPSenatAbbreviation>();

        public MPSenat()
        {

        }

        public MPSenat(string senatName, int senatCategory, int sorting)
        {
            MPSenatName = senatName;
            MPCategorieID = senatCategory;
            MPSenatSorting = sorting;
        }
    }
}
