using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes
{
    public class SenatKW
    {
        public int Sortierung { get; set; }
        public string Rohdaten { get; set; }
        public string SenatBezeichnung { get; set; }


        public SenatKW(int sortierung, string rohdaten, string senatBezeichnung)
        {
            Sortierung = sortierung;
            Rohdaten = rohdaten;
            SenatBezeichnung = senatBezeichnung;
        }
    }
}
