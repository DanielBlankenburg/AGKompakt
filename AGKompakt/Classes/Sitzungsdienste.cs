using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes
{
    public class Sitzungsdienste
    {
        public int ID { get; set; }
        public string RohDatum { get; set; }
        public string AnzeigeDatum { get; set; }
        public int HiWiZitat { get; set; }
        public int HiWiGuide { get; set; }
        public string HiWiNameZitat { get; set; }
        public string HiWiNameGuide { get; set; }
        public string Jahr { get; set; }
        public string Monat { get; set; }
        public string Tag { get; set; }
        public string Struchgruppe { get; set; }

        public Sitzungsdienste(int id, string rohdatum, string anzeigeDatum, int hiWiZitat, string hiWiNameZitat, string jahr, string monat, string tag, string spruchgruppe, int hiWiGuide, string hiwiNameGuide)
        {
            ID = id;
            RohDatum = rohdatum;
            AnzeigeDatum = anzeigeDatum;
            HiWiZitat = hiWiZitat;
            HiWiNameZitat = hiWiNameZitat;
            Jahr = jahr;
            Monat = monat;
            Tag = tag;
            Struchgruppe = spruchgruppe;
            HiWiGuide = hiWiGuide;
            HiWiNameGuide = hiwiNameGuide;
        }
    }
}
