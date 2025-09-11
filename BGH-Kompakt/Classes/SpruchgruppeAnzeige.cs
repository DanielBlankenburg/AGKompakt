using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes
{
    public class SpruchgruppeAnzeige
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public string Titel { get; set; }

        public string Dienstbezeichnung { get; set; }

        public string Name_Voll { get; set; }

        public SpruchgruppeAnzeige(string id, string nachname, string titel, string dienstbezeichnung)
        {
            ID = id;
            Name = nachname;
            Titel = titel;
            Dienstbezeichnung = dienstbezeichnung;
            Name_Voll = !(titel != string.Empty) ? Name : titel + " " + Name;
            Dienstbezeichnung = dienstbezeichnung;
        }
    }
}
