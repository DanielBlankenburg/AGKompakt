using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes._LookUp.MP
{
    public class MPSenatAbbreviation
    {
        public int MPSenatAbbreviationID { get; set; }
        public string MPSenatAbbreviationText { get; set; }
        public ICollection<MPSenat> Senate {  get; set; } = new List<MPSenat>();

        public MPSenatAbbreviation()
        {
            
        }

        public MPSenatAbbreviation(string mPSenatAbbreviationText)
        {
            MPSenatAbbreviationText = mPSenatAbbreviationText;
        }
    }
}
