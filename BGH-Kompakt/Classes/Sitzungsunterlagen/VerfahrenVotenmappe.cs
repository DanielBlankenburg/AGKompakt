using BGH_Kompakt.Classes.Senate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes.Sitzungsunterlagen
{
    public class VerfahrenVotenmappe
    {
        public int VerfahrenVotenmappeID { get; set; }
        public string Verfahren_FullPath { get; set; } = string.Empty;
        public string Verfahren_Anzeigedaten { get; set; } = string.Empty;
        public Senat Senat { get; set; }

        public VerfahrenVotenmappe()
        {
        }

        public void FillVerfahren(Verfahren verfahren, Senat senat)
        {
            Verfahren_FullPath = verfahren.Verfahren_FullPath;
            Verfahren_Anzeigedaten = verfahren.Verfahren_Anzeigedaten;
            Senat = senat;
        }
    }
}
