using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes
{
    public class Spruchgruppe
    {
        public int ID { get; set; }

        public int RichterID { get; set; }

        public Spruchgruppe(int id, int richterID)
        {
            ID = id;
            RichterID = richterID;
        }
    }
}
