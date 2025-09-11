using BGH_Kompakt.Classes._LookUp.MP;
using BGH_Kompakt.Classes.Senate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

#nullable enable

namespace BGH_Kompakt.Classes.MP
{
    public class MPDecision
    {
        public int MPDecisionID { get; set; }
        public string Rohdaten { get; set; }
        public DateTime Date { get; set; }
        public int Typ { get; set; }  //1=Urteil oder 2=Beschluss
        public string? Rechtsgebiet { get; set; } //z.B. Zwangsvollstreckung, Familiensache, Insolvenzverfahren
        public string? Normenkette { get; set; }
        public string? Leitsatz { get; set; }
        public string Aktenzeichen { get; set; }
        public int SenatID { get; set; }
        public MPSenat Senat { get; set; }
        public string RegZeichen { get; set; }
        public string LaufendeNummer { get; set; }
        public string Jahr { get; set; }
        public string? InstanzErste { get; set; }
        public string? InstanzZweite { get; set; }
        public int MPWeekID { get; set; }
        public MPWeek MPWeek { get; set; }
        public string PathName { get; set; }
        public string FileName { get; set; }
        public MPBE BE { get; set; }
        public string Vermerk { get; set; }
        public bool VermerkAnzeige { get; set; } = false;
        [NotMapped]
        public bool EMailVersenden { get; set; } = true;

        public string TypAnzeige
        {
            get
            {
                string Anzeige = string.Empty;
                switch (Typ)
                {
                    case 0:
                        Anzeige = "unbekannt";
                        break;
                    case 1:
                        Anzeige = "Urteil";
                        break;
                    case 2:
                        Anzeige = "Beschluss";
                        break;
                }
                return Anzeige;
            }
        }
        public bool VermerkVorhanden
        {
            get
            {
                return Vermerk != string.Empty && Vermerk != null;
            }
        }

    }
}
