using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes
{
    public class BereichMP
    {
        public int Bereich { get; set; }

        public string Bereich_Path { get; set; }
        public BereichMP(int bereich)
        {
            string Anzeige = string.Empty;
            Bereich = bereich;
            switch (bereich)
            {
                case 1:
                    Anzeige = "Zivilsenate";
                    break;
                case 2:
                    Anzeige = "Strafsenate";
                    break;
                case 3:
                    Anzeige = "Sondersenate";
                    break;
            }
            Bereich_Path = Anzeige;
        }
    }
}
