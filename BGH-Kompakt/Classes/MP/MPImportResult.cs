using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes.MP
{
    public class MPImportResult
    {
        public string Name { get; set; }
        public bool Importpdf { get; set; } = false;
        public bool ImportWord { get; set; } = false;
    }
}
