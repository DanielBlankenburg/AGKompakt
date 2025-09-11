using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes
{
    public class Richter
    {
        public int ID { get; set; }

        public string Vorname { get; set; }

        public string Nachname { get; set; }

        public string Titel { get; set; }

        public string E_Mail { get; set; }

        public string Dienstbezeichnung { get; set; }

        public int Status { get; set; }

        public Richter(
          int id,
          string vorname,
          string nachname,
          string titel,
          string email,
          string dienstbezeichnung,
          int status)
        {
            ID = id;
            Vorname = vorname;
            Nachname = nachname;
            Titel = titel;
            E_Mail = email;
            Dienstbezeichnung = dienstbezeichnung;
            Status = status;
        }
    }
}
