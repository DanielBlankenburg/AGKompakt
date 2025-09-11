using BGH_Kompakt.Classes._LookUp.MP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes.MP
{
    public class MPDecisionFile
    {
        public string FileName { get; set; }
        public int Bereich { get; set; }
        public string SenatRohstring { get; set; }
        public MPSenat Senat { get; set; }
        public string Path { get; set; }
    }
}
