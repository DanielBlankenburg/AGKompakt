using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes
{
    public class Kalenderwoche
    {
        public string Rohdaten { get; set; }

        public string Path { get; set; }
        public string Anzeige { get; set; }

        public string Jahr { get; set; }
        public string KW { get; set; }

        public Kalenderwoche(string rohdaten, string path,string jahr)
        {
            Rohdaten = rohdaten;
            Path = path;
            Jahr = jahr;
            KW = rohdaten.Substring(rohdaten.Length - 2, 2);
            Anzeige = "KW " + KW;
        }
    }
}
